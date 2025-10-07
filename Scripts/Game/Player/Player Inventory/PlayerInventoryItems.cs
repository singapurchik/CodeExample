using System.Collections.Generic;
using FAS.Sounds;
using FAS.Items;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerInventoryItems : PlayerInventoryObject
	{
		[Inject] private InteractionUsedItemDisplayer _usedItemDisplayer;
		[Inject] private ItemInventoryModels _inventoryItemModels;
		[Inject] private InventoryDetailed _inventoryDetailed;
		[Inject] private ISoundEffectsPlayer _soundEffects;
		
		private readonly Dictionary<ItemType, PlayerInventorySlot> _slots = new (10);
		
		public bool IsHas(ItemType item) => _slots.ContainsKey(item);
		
		public bool TryGetData(ItemType type, out IItemData data)
		{
			if (_slots.TryGetValue(type, out var slot))
			{
				data = slot.Item;
				return data != null;
			}
			
			data = null;
			return false;
		}
		
		public bool TryAdd(ItemData data, out PlayerInventorySlot currentSlot)
		{
			if (_slots.TryGetValue(data.Type, out var slot))
			{
				Debug.Log("Is has slot");
				currentSlot = slot;
					
				if (data.IsStackable)
					currentSlot.IncreaseItems();

				return false;
			}
			else
			{
				Debug.Log("Create slot");
				
				_inventoryItemModels.TryCreateModel(data.InventoryModel, data.Type);
				currentSlot = CreateSlot(data);
				_slots.Add(data.Type, currentSlot);
				return true;
			}
		}
		
		public bool TryRemove(ItemType itemType)
		{
			if (_slots.TryGetValue(itemType, out var slot))
			{
				DecreaseItemInSlot(slot);
				
				var item = slot.Item;

				if (item.IsCanUseFromInventory)
				{
					_inventoryDetailed.TryStartHidingView();
				}
				else if (item.IsUsingInInteraction && _inventoryItemModels.TryGetModel(itemType, out var model))
				{
					_soundEffects.PlayOneShot(item.InteractionUseSound);
					_usedItemDisplayer.ShowUsedItem(item, model);
				}
				return true;
			}
			return false;
		}
		
		private void DecreaseItemInSlot(PlayerInventorySlot slot)
		{
			slot.DecreaseItems();

			if (!slot.IsHasItems)
			{
				slot.Clear();
				_slots.Remove(slot.Item.Type);
				InventoryQuickBar.RequestUpdateSlotsView();
				InvokeOnSlotCleared(slot);
			}
		}
	}
}