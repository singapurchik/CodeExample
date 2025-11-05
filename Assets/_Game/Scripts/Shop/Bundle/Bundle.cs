using UnityEngine.UI;
using UnityEngine;
using Zenject;
using System;
using TMPro;
using Core;

namespace Shop
{
	public class Bundle : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _costsRewardsText;
		[SerializeField] private BundleBuyButton _buyButton;
		[SerializeField] private Button _infoButton;

		[Inject] private BundlePhraseFormatter _formatter;
		[Inject] private IPlayerDataInfo _playerData;

		private BundleData _currentData;

		public event Action<BundleData> OnInfoButtonClicked;
		public event Action<BundleData> OnBuyButtonClicked;
		public event Action<Bundle> OnBundleOutOfStock;

		public void Initialize(BundleData data)
		{
			_infoButton.onClick.AddListener(InvokeOnInfoButtonClicked);
			_buyButton.AddListenerOnClick(InvokeOnBuyButtonClicked);
			
			_currentData = data;
			_costsRewardsText.text =
				_formatter.Build(new CostOperationsView(data.Costs), 
					new RewardOperationsView(data.Rewards));

			UpdateButtonState();
		}

		public void UpdateButtonState()
		{
			if (CanPay(_currentData, _playerData))
				_buyButton.Enable();
			else
				_buyButton.Disable();
		}

		private static bool CanPay(BundleData data, IPlayerDataInfo info)
		{
			foreach (var costEntry in data.Costs)
				if (!CanApplyConsumeEntry(costEntry, info))
					return false;
			
			return true;
		}
		
		private static bool CanApplyConsumeEntry(CostEntry entry, IPlayerDataInfo info)
		{
			if (entry.Operation)
			{
				if (entry.Operation is IOperationWithParameter operationWithParameter && entry.Parameter != null)
				{
					if (!operationWithParameter.IsSupports(entry.Parameter))
					{
						Debug.LogWarning($"[Shop] Param of type {entry.Parameter.GetType().Name} " +
						                 $"is not supported by {entry.Operation.name}. Fallback to default.");
						return entry.Operation.IsCanApply(info);
					}
					return operationWithParameter.IsCanApply(info, entry.Parameter);
				}
				return entry.Operation.IsCanApply(info);
			}
			return false;
		}
		
		private void BundleOutOfStock()
		{
			_infoButton.onClick.RemoveListener(InvokeOnInfoButtonClicked);
			_buyButton.RemoveListenerOnClick(InvokeOnBuyButtonClicked);
			OnBundleOutOfStock?.Invoke(this);
		}
		
		private void InvokeOnBuyButtonClicked()
		{
			_buyButton.SetProcessingText();
			OnBuyButtonClicked?.Invoke(_currentData);
		}
		
		private void InvokeOnInfoButtonClicked() => OnInfoButtonClicked?.Invoke(_currentData);
	}
}