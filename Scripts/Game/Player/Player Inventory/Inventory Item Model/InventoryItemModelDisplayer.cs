using UnityEngine;
using FAS.Items;
using Zenject;
using System;

namespace FAS.Players
{
	public class InventoryItemModelDisplayer : MonoBehaviour
	{
		[SerializeField] private Transform _modelsHolder;
		
		[Inject] private RangeWeaponInventoryModels _rangeModels;
		[Inject] private MeleeWeaponInventoryModels _meleeModels;
		[Inject] private InventoryItemModelScreen _screen;
		[Inject] private AmmoInventoryModels _ammoModels;
		[Inject] private ItemInventoryModels _itemModels;

		private InventoryItemModel _displayingItemModel;

		public IInventoryModelRotatable DisplayingItemModel => _displayingItemModel;

		private bool _isActive;

		public event Action OnActivate;
		public event Action OnShow;

		private void Awake()
		{
			_rangeModels.Initialize(_modelsHolder);
			_meleeModels.Initialize(_modelsHolder);
			_itemModels.Initialize(_modelsHolder);
			_ammoModels.Initialize(_modelsHolder);
		}

		private void OnEnable()
		{
			_screen.AddOnHiddenListener(ClearAndDisable);
		}

		private void OnDisable()
		{
			_screen.RemoveOnHiddenListener(ClearAndDisable);
		}

		public void Show(BaseItemData data)
		{
			if (data.TryResolveModel(_itemModels, _rangeModels, _meleeModels, _ammoModels, out var model))
				Show(model);
		}

		public void Show(InventoryItemModel itemModel)
		{
			if (_displayingItemModel != null && _displayingItemModel != itemModel)
			{
				_displayingItemModel.Hide();
				_displayingItemModel = null;
			}

			if (_displayingItemModel == null)
			{
				_displayingItemModel = itemModel;

				if (_isActive)
				{
					_displayingItemModel.Show();
				}
				else
				{
					_screen.Show();
					_displayingItemModel.ShowWithAnim(_screen.ShowFadeDuration);
					OnActivate?.Invoke();
					_isActive = true;
				}

				OnShow?.Invoke();
			}
		}

		public void Hide()
		{
			_screen.Hide();
			if (_displayingItemModel != null)
				_displayingItemModel.HideWithAnim(_screen.HideFadeDuration);
		}

		public void ClearAndDisable()
		{
			_isActive = false;
			if (_displayingItemModel != null)
			{
				_displayingItemModel.gameObject.SetActive(false);
				_displayingItemModel = null;
			}
		}
	}
}
