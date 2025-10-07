using FAS.Players;
using Zenject;

namespace FAS
{
	public class PlayerWeaponEquipper
	{
		[Inject] private IInventoryDetailedUpdater _inventoryDetailed;
		[Inject] private IReadOnlyPlayerInventory _inventoryInfo;
		[Inject] private PlayerWeaponHolder _weaponHolder;
		
		public void ChangeRangeWeapon(RangeWeapon nextWeapon)
		{
			TryUnequipRangeWeapon();
			EquipRangeWeapon(nextWeapon);
		}
		
		public void ChangeMeleeWeapon(MeleeWeapon nextWeapon)
		{
			TryUnequipMeleeWeapon();
			EquipMeleeWeapon(nextWeapon);
		}
		
		public void OnTryEquipFromInventory()
		{
			if (TryEquipWeapon())
				_inventoryDetailed.UpdateDetails();
		}

		public void OnTryUnequipFromInventory()
		{
			if (TryUnequipWeapon())
				_inventoryDetailed.UpdateDetails();
		}

		private bool TryUnequipWeapon()
		{
			if (_inventoryInfo.TryGetItemDataFromSelectedSlot<IRangeWeaponData>(out var rangeWeaponData))
			{
				TryUnequipRangeWeapon();
				return true;
			}
			
			if (_inventoryInfo.TryGetItemDataFromSelectedSlot<IMeleeWeaponData>(out var meleeWeaponData))
			{
				TryUnequipMeleeWeapon();
				return true;
			}
			
			return false;
		}
		
		private bool TryEquipWeapon()
		{
			if (_inventoryInfo.TryGetItemDataFromSelectedSlot<IRangeWeaponData>(out var rangeWeaponData))
			{
				foreach (var weapon in _weaponHolder.RangeWeapons)
				{
					if (weapon.Data.RangeWeaponType == rangeWeaponData.RangeWeaponType)
					{
						ChangeRangeWeapon(weapon);
						return true;
					}
				}
			}
			else if (_inventoryInfo.TryGetItemDataFromSelectedSlot<IMeleeWeaponData>(out var meleeWeaponData))
			{
				foreach (var weapon in _weaponHolder.MeleeWeapons)
				{
					if (weapon.Data.MeleeWeaponType == meleeWeaponData.MeleeWeaponType)
					{
						ChangeMeleeWeapon(weapon);
						return true;
					}
				}
			}
			return false;
		}
		
		private void TryUnequipRangeWeapon()
		{
			if (_weaponHolder.CurrentRangeWeapon)
				_weaponHolder.CurrentRangeWeapon.Unequip();
		}
		
		private void TryUnequipMeleeWeapon()
		{
			if (_weaponHolder.CurrentMeleeWeapon)
				_weaponHolder.CurrentMeleeWeapon.Unequip();
		}
		
		private void EquipRangeWeapon(RangeWeapon nextWeapon)
		{
			_weaponHolder.SetCurrentWeapon(nextWeapon);
			nextWeapon.Equip();
			SetRangeWeaponToSpine();
		}

		private void EquipMeleeWeapon(MeleeWeapon nextWeapon)
		{
			_weaponHolder.SetCurrentWeapon(nextWeapon);
			nextWeapon.Equip();
			SetMeleeWeaponToSpine();
		}

		public void SetRangeWeaponToRightHand(float duration = 0)
			=> _weaponHolder.CurrentRangeWeapon.SetToRightHand(duration);
		
		public void SetMeleeWeaponToRightHand(float duration = 0)
			=> _weaponHolder.CurrentMeleeWeapon.SetToRightHand(duration);
		
		public void SetRangeWeaponToLeftHand(float duration = 0)
			=> _weaponHolder.CurrentRangeWeapon.SetToLeftHand(duration);
		
		public void SetMeleeWeaponToLeftHand(float duration = 0)
			=> _weaponHolder.CurrentMeleeWeapon.SetToLeftHand(duration);
		
		public void SetRangeWeaponToSpine(float duration = 0)
			=> _weaponHolder.CurrentRangeWeapon.SetToSpine(duration);
		
		public void SetMeleeWeaponToSpine(float duration = 0)
			=> _weaponHolder.CurrentMeleeWeapon.SetToSpine(duration);
	}
}