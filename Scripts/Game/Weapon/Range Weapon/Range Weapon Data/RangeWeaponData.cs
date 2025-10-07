using FAS.Players;
using UnityEngine;

namespace FAS
{
	[CreateAssetMenu(fileName = "Range Weapon Data", menuName = "FAS/Item Data/Range Weapon", order = 2)]
	public class RangeWeaponData : WeaponData, IRangeWeaponData
	{
		[SerializeField] private RangeWeaponType _rangeWeaponType;
		[SerializeField] private int _maxAmmo = 1;

		public RangeWeaponType RangeWeaponType => _rangeWeaponType;
		public override WeaponType WeaponType => WeaponType.Range;
		
		public int MaxAmmo => _maxAmmo;

		public override bool TryResolveModel(ItemInventoryModels items, RangeWeaponInventoryModels range,
			MeleeWeaponInventoryModels melee, AmmoInventoryModels ammo, out InventoryItemModel model)
			=> range.TryGetModel(_rangeWeaponType, out model);

		public override bool TryAddTo(PlayerInventoryWeapons weapons, PlayerInventoryItems items,
			PlayerInventoryAmmo ammo, out PlayerInventorySlot slot)
			=> weapons.TryAdd(this, out slot);

		public override bool TryRemoveFrom(PlayerInventoryWeapons weapons, PlayerInventoryItems items,
			PlayerInventoryAmmo ammo)
			=> weapons.TryRemove(_rangeWeaponType);
	}
}