using System.Collections.Generic;
using Zenject;

namespace FAS.Players
{
	public class PlayerInventoryAmmo : PlayerInventoryObject
	{
		[Inject] private AmmoInventoryModels _ammoModels;
		
		private readonly Dictionary<AmmoType, PlayerInventorySlot> _ammoSlots = new (10);
		
		public bool TryAdd(AmmoData data, out PlayerInventorySlot slot)
		{
			if (_ammoSlots.TryGetValue(data.AmmoType, out var existing))
			{
				slot = existing;
				return false;
			}

			_ammoModels.TryCreateModel(data.InventoryModel, data.AmmoType);
			slot = CreateSlot(data);
			_ammoSlots.Add(data.AmmoType, slot);
			return true;
		}
		
		public bool TryRemove(AmmoType type)
		{
			if (_ammoSlots.TryGetValue(type, out var slot))
			{
				DecreaseItemInSlot(slot);
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
				_ammoSlots.Remove(slot.Item.Type);
				InventoryQuickBar.RequestUpdateSlotsView();
				InvokeOnSlotCleared(slot);
			}
		}
	}
}