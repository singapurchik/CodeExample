using System.Collections.Generic;
using UnityEngine;
using FAS.Items;
using VInspector;
using Zenject;

namespace FAS.Players
{
	public class PlayerInventory : MonoBehaviour, IPlayerInventoryAdd, IReadOnlyPlayerInventory
	{
		[SerializeField] private List<BaseItemData> _itemsOnStartLevel = new ();

		[Inject] private ItemInventoryModels _inventoryItemModels;
		[Inject] private RangeWeaponInventoryModels _rangeModels;
		[Inject] private MeleeWeaponInventoryModels _meleeModels;
		[Inject] private InventoryDetailed _inventoryDetailed;
		[Inject] private AmmoInventoryModels _ammoModels;

		[Inject] public PlayerInventoryWeapons Weapons { get; private set; }
		[Inject] public PlayerInventoryItems Items { get; private set; }
		[Inject] public PlayerInventoryAmmo Ammo { get; private set; }

		private PlayerInventorySlot _selectedSlot;

		private void OnEnable()
		{
			_inventoryDetailed.OnHidden += OnModelViewHidden;
			Items.OnSlotCleared += OnSlotCleared;
		}

		private void OnDisable()
		{
			_inventoryDetailed.OnHidden -= OnModelViewHidden;
			Items.OnSlotCleared -= OnSlotCleared;
		}

		private void Start()
		{
			foreach (var data in _itemsOnStartLevel)
				TryAdd(data, false);
		}

		private void OnSlotCleared(PlayerInventorySlot slot)
		{
			slot.OnSelected -= OnSlotSelected;
		}

		public bool TryGetItemDataFromSelectedSlot<TInterface>(out TInterface data)
			where TInterface : class, IItemData
		{
			data = _selectedSlot?.Item as TInterface;
			return data != null;
		}

		public bool TryGetItemData(ItemType type, out IItemData data)
		{
			if (Items.TryGetData(type, out var itemData))
			{
				data = itemData;
				return true;
			}
			data = null;
			return false;
		}

		public void TryAdd(BaseItemData data, bool isAutoSelect)
		{
			if (data.TryAddTo(Weapons, Items, Ammo, out var slot))
				slot.OnSelected += OnSlotSelected;
			
			if (isAutoSelect)
			{
				slot.TrySelect();
				OnSlotSelected(slot);
			}
		}

		private void OnSlotSelected(PlayerInventorySlot slot)
		{
			if (_selectedSlot != null && _selectedSlot != slot)
				_selectedSlot.TryUnselect();

			_selectedSlot = slot;
			_selectedSlot.TrySelect();

			TryShowItem(_selectedSlot.Item);
		}

		private void OnModelViewHidden()
		{
			if (_selectedSlot != null)
			{
				_selectedSlot.TryUnselect();
				_selectedSlot = null;	
			}
		}

		private void TryShowItem(BaseItemData data)
		{
			if (data.TryResolveModel(_inventoryItemModels, _rangeModels, _meleeModels, _ammoModels, out var model))
				_inventoryDetailed.Show(model, data);
		}

#if UNITY_EDITOR
		[Button]
		public void TryAddEditor(BaseItemData data, bool isAutoSelect) => TryAdd(data, isAutoSelect);
#endif
	}
}