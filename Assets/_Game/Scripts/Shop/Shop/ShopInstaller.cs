using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Core;

#if UNITY_EDITOR
using Core.Editor;
#endif

namespace Shop
{
	public sealed class ShopInstaller : MonoInstaller
	{
		[Header("Common")]
		[SerializeField] private ShopView _view;
		[SerializeField] private List<PlayerDataValueInfo> _dataValueInfos = new();

		[Header("Stats")]
		[SerializeField] private StatsViewsPool _statsViewsPool;
		[SerializeField] private List<StatPlusBinding> _statPlusBindings = new ();
		
		[Header("Bundles")]
		[SerializeField] private BundlesPool _bundlesPool;
		[SerializeField] private List<BundleData> _bundles = new();
		[SerializeField] private SelectedBundleDataReference _selectedReference;
		[SerializeField] private BundlesSourceMode _mode = BundlesSourceMode.FullList;
		
		[Header("Scenes")]
		[SerializeField] private SceneLoadingData _bundleDetailedScene;
		[SerializeField] private SceneLoadingData _shopScene;
		
		private readonly Shop _shop = new ();
		
		public override void InstallBindings()
		{
			var scenesLoader = new ShopScenesLoader(_bundleDetailedScene, _shopScene);
			var backend = new Backend();

			Container.QueueForInject(_shop);

			switch (_mode)
			{
				case BundlesSourceMode.FullList:
					Container.Bind<IBundleSource>()
						.To<ListBundleSource>()
						.AsSingle()
						.WithArguments((IReadOnlyList<BundleData>)_bundles);
					break;

				case BundlesSourceMode.SingleSelected: 
					
					if (_selectedReference.Data == null)
						_selectedReference.Set(_bundles[0]);

					Container.Bind<IBundleSource>()
						.To<SingleBundleSource>()
						.AsSingle()
						.WithArguments(_selectedReference.Data);
					break;
			}

			Container.BindInstance((IReadOnlyList<StatPlusBinding>)_statPlusBindings).WhenInjectedIntoInstance(_shop);
			Container.Bind<IShopScenesLoader>().FromInstance(scenesLoader).WhenInjectedIntoInstance(_shop);
			Container.Bind<IReadOnlyList<PlayerDataValueInfo>>().FromInstance(_dataValueInfos).AsSingle();

			Container.BindInstance(_selectedReference).WhenInjectedIntoInstance(_shop);

			Container.Bind<IShopView>().FromInstance(_view).WhenInjectedIntoInstance(_shop);
			Container.BindInstance(_statsViewsPool).WhenInjectedIntoInstance(_view);
			Container.BindInstance(_bundlesPool).WhenInjectedIntoInstance(_view);

			Container.Bind<BundlePhraseFormatter>().WhenInjectedInto<Bundle>();

			Container.BindInstance(backend).WhenInjectedIntoInstance(_shop);
		}

		public override void Start()
		{
			_shop.Initialize();
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			_dataValueInfos.MakeSlotsEmptyAndPreventDuplicates();
		}
#endif
	}
}