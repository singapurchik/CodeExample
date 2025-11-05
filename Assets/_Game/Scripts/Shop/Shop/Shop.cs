using System.Collections.Generic;
using System.Collections;
using Zenject;
using Core;

namespace Shop
{
	public sealed class Shop
	{
		[Inject] private SelectedBundleDataReference _selectedBundleDataReference;
		[Inject] private IReadOnlyList<StatPlusBinding> _plusBindings;
		[Inject] private ICoroutineRunner _coroutineRunner;
		[Inject] private IShopScenesLoader _scenesLoader;
		[Inject] private IBundleSource _bundleSource;
		[Inject] private PlayerData _playerData;
		[Inject] private Backend _backend;
		[Inject] private IShopView _view;
		
		private Dictionary<PlayerDataValueInfo, StatPlusBinding> _plusMap;

		public void Initialize()
		{
			_plusMap = new Dictionary<PlayerDataValueInfo, StatPlusBinding>(_plusBindings?.Count ?? 0);
			if (_plusBindings != null)
			{
				for (int i = 0; i < _plusBindings.Count; i++)
				{
					var b = _plusBindings[i];
					if (b.Info && b.Operation)
						_plusMap[b.Info] = b;
				}
			}
			
			var bundles = _bundleSource.GetBundles();
			if (bundles != null && bundles.Count > 0)
			{
				for (var i = 0; i < bundles.Count; i++)
				{
					var bundle = _view.CreateBundle(bundles[i]);
					bundle.OnInfoButtonClicked += OnShowBundleInfoButtonClicked;
					bundle.OnBundleOutOfStock += OnBundleOutOfStock;
					bundle.OnBuyButtonClicked += OnBuyBundle;
				}
			}

			_view.CreateStatsViews();
			_view.OnCloseInfoButtonClicked += OnCloseBundleInfoButtonClicked;
			_view.OnPlusButtonClicked += OnStatPlusClicked;
			_view.UpdateView();
		}
		
		private void OnStatPlusClicked(PlayerDataValueInfo info)
		{
			if (!_plusMap.TryGetValue(info, out var bind) || bind.Operation == null)
				return;

			ApplyOperation(bind);
			_view.UpdateView();
		}
		
		
		private void ApplyOperation(StatPlusBinding b)
		{
			if (b.Operation is IOperationWithParameter withParam && b.Param != null && withParam.IsSupports(b.Param))
				withParam.Apply(_playerData, b.Param);
			else
				b.Operation.Apply(_playerData);
		}
		
		private void OnCloseBundleInfoButtonClicked()
		{
			_view.DisableInput();
			_scenesLoader.LoadShopScene();
		}
		
		private void OnShowBundleInfoButtonClicked(BundleData data)
		{
			_view.DisableInput();
			_selectedBundleDataReference.Set(data);
			_scenesLoader.LoadBundleInfoScene();
		}

		private void OnBundleOutOfStock(Bundle bundle)
		{
			bundle.OnInfoButtonClicked -= OnShowBundleInfoButtonClicked;
			bundle.OnBundleOutOfStock -= OnBundleOutOfStock;
			bundle.OnBuyButtonClicked -= OnBuyBundle;
		}
		
		private void OnBuyBundle(BundleData data)
		{
			if (!_backend.IsBusy)
				_coroutineRunner.Run(BuyRoutine(data));
		}
		
		private IEnumerator BuyRoutine(BundleData data)
		{
			_view.DisableInput();
			
			yield return _backend.SendRequestRoutine();

			foreach (var costEntry in data.Costs)
				ApplyConsumeEntry(costEntry, _playerData);

			foreach (var rewardEntry in data.Rewards)
				ApplyProvideEntry(rewardEntry, _playerData);

			_view.UpdateView();
			_view.EnableInput();
		}
		
		private static void ApplyConsumeEntry(CostEntry entry, PlayerData data)
		{
			if (entry.Operation)
			{
				if (entry.Operation is IOperationWithParameter operationWithParameter && entry.Parameter != null)
				{
					if (operationWithParameter.IsSupports(entry.Parameter))
					{
						operationWithParameter.Apply(data, entry.Parameter);
						return;
					}
				}

				entry.Operation.Apply(data);	
			}
		}

		private static void ApplyProvideEntry(RewardEntry entry, PlayerData data)
		{
			if (entry.Operation)
			{
				if (entry.Operation is IOperationWithParameter operationWithParameter && entry.Parameter != null)
				{
					if (operationWithParameter.IsSupports(entry.Parameter))
					{
						operationWithParameter.Apply(data, entry.Parameter);
						return;
					}
				}

				entry.Operation.Apply(data);	
			}
		}
	}
}