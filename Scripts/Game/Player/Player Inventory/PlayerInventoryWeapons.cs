using System.Collections.Generic;
using FAS.Sounds;
using Zenject;

namespace FAS.Players
{
	public class PlayerInventoryWeapons : PlayerInventoryObject
	{
		[Inject] private InteractionUsedItemDisplayer _usedItemDisplayer;
		[Inject] private RangeWeaponInventoryModels _rangeModels;
		[Inject] private MeleeWeaponInventoryModels _meleeModels;
		[Inject] private InventoryDetailed _inventoryDetailed;
		[Inject] private ISoundEffectsPlayer _soundEffects;
		
		private readonly Dictionary<RangeWeaponType, PlayerInventorySlot> _rangeWeaponSlots = new (10);
		private readonly Dictionary<MeleeWeaponType, PlayerInventorySlot> _meleeWeaponSlots = new (10);

		public bool TryAdd(RangeWeaponData data, out PlayerInventorySlot slot)
		{
			if (_rangeWeaponSlots.TryGetValue(data.RangeWeaponType, out var existing))
			{
				slot = existing;
				return false;
			}

			_rangeModels.TryCreateModel(data.InventoryModel, data.RangeWeaponType);
			slot = CreateSlot(data);
			_rangeWeaponSlots.Add(data.RangeWeaponType, slot);
			return true;
		}

		public bool TryAdd(MeleeWeaponData data, out PlayerInventorySlot slot)
		{
			if (_meleeWeaponSlots.TryGetValue(data.MeleeWeaponType, out var existing))
			{
				slot = existing;
				return false;
			}

			_meleeModels.TryCreateModel(data.InventoryModel, data.MeleeWeaponType);
			slot = CreateSlot(data);
			_meleeWeaponSlots.Add(data.MeleeWeaponType, slot);
			return true;
		}
		
		public bool TryRemove(RangeWeaponType weaponType)
		{
			if (_rangeWeaponSlots.Remove(weaponType, out var slot))
			{
				OnRemovedWeapon(slot);
				return true;
			}
			return false;
		}
		
		public bool TryRemove(MeleeWeaponType weaponType)
		{
			if (_meleeWeaponSlots.Remove(weaponType, out var slot))
			{
				OnRemovedWeapon(slot);
				return true;
			}
			return false;
		}

		private void OnRemovedWeapon(PlayerInventorySlot slot)
		{
			slot.DecreaseItems();
			slot.Clear();
			_slots.Remove(slot.Item.Type);
			InventoryQuickBar.RequestUpdateSlotsView();
			InvokeOnSlotCleared(slot);
		}
	}
}