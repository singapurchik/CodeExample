using UnityEngine;
using FAS.Items;
using Zenject;
using System;

namespace FAS.Players
{
	public class InventoryDetailed : MonoBehaviour, IInventoryDetailedUpdater
	{
		[Inject] private IInventoryDetailedScreenGroup _inventoryDetailedScreensGroup;
		[Inject] private InventoryItemModelDisplayer _itemModelDisplayer;
		[Inject] private InventoryItemModelRotator _itemModelRotator;
		[Inject] private IReadOnlyInputEvents _inputEvents;
		[Inject] private IInputVisibility _inputVisibility;
		[Inject] private InventoryDetailedScreen _screen;
		
		private IItemData _currentItemData;

		private bool _isShown;
		
		public event Action OnHidden;

		private void OnEnable()
		{
			_screen.OnCloseButtonClicked += TryStartHidingView;
			_screen.OnDescriptionHidden += OnDescriptionHidden;
			_screen.AddOnStartHideListener(OnViewStartHide);
			_screen.AddOnHiddenListener(InvokeOnViewHidden);
			_screen.OnDescriptionShown += OnDescriptionShow;
		}

		private void OnDisable()
		{
			_screen.OnCloseButtonClicked -= TryStartHidingView;
			_screen.OnDescriptionHidden -= OnDescriptionHidden;
			_screen.RemoveOnStartHideListener(OnViewStartHide);
			_screen.RemoveOnHiddenListener(InvokeOnViewHidden);
			_screen.OnDescriptionShown -= OnDescriptionShow;
		}
		
		private void Start()
		{
			_inventoryDetailedScreensGroup.Set(_screen);
		}
		
		public void UpdateDetails()
		{
			UpdateEquipButtons();
		}

		private void OnDescriptionHidden()
		{
			_inputVisibility.TryShowInventoryExamineItemButton();
			
			if (_currentItemData.IsCanUseFromInventory)
				_inputVisibility.TryShowUseInventoryItemButton();

			if (_currentItemData.IsCanEquipFromInventory)
				UpdateEquipButtons();
		}

		private void UpdateEquipButtons()
		{
			if (_currentItemData.IsEquipped)
			{
				_inputVisibility.TryShowInventoryUnequipItemButton();
				_inputVisibility.TryHideInventoryEquipItemButton();
			}
			else
			{
				_inputVisibility.TryHideInventoryUnequipItemButton();
				_inputVisibility.TryShowInventoryEquipItemButton();
			}
		}
		
		private void OnDescriptionShow()
		{
			_inputVisibility.TryHideInventoryExamineItemButton();
			_inputVisibility.TryHideInventoryUnequipItemButton();
			_inputVisibility.TryHideInventoryEquipItemButton();
			_inputVisibility.TryHideUseInventoryItemButton();
		}

		public void Show(InventoryItemModel itemModel, IItemData itemData)
		{
			_currentItemData = itemData;
			
			if (!_isShown)
			{
				_itemModelDisplayer.OnActivate += OnItemModelDisplayerActivate;
				_itemModelDisplayer.OnShow += OnItemModelShow;
				_isShown = true;
			}
			
			_itemModelDisplayer.Show(itemModel);
		}
		
		private void OnItemModelDisplayerActivate()
		{
			_inventoryDetailedScreensGroup.Show();
		}

		private void OnItemModelShow()
		{
			if (_currentItemData.IsCanUseFromInventory)
				_inputVisibility.TryShowUseInventoryItemButton();
			else
				_inputVisibility.TryHideUseInventoryItemButton();
			
			if (_currentItemData.IsCanEquipFromInventory)
			{
				UpdateEquipButtons();
			}
			else
			{
				_inputVisibility.TryHideInventoryUnequipItemButton();
				_inputVisibility.TryHideInventoryEquipItemButton();
			}
			
			_screen.UpdateScreen(_currentItemData.Name, _currentItemData.Description);
			_itemModelRotator.SetCurrentRotatable(_itemModelDisplayer.DisplayingItemModel);
		}
		
		public void TryStartHidingView()
		{
			if (_screen.IsDescriptionShown)
			{
				_screen.TryHideDescription();
			}
			else
			{
				if (_screen.IsShown && !_screen.IsProcessHide)
				{
					_itemModelRotator.StopRotating();
					_itemModelDisplayer.Hide();
					_inventoryDetailedScreensGroup.Hide();
				}	
			}
		}

		private void OnViewStartHide()
		{
			_itemModelDisplayer.OnActivate -= OnItemModelDisplayerActivate;
			_itemModelDisplayer.OnShow -= OnItemModelShow;
			_isShown = false;
		}
		
		private void InvokeOnViewHidden() => OnHidden?.Invoke();
	}
}