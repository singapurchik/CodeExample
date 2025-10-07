using FAS.Players;
using UnityEngine;

namespace FAS
{
	[CreateAssetMenu(fileName = "Melee Weapon Data", menuName = "FAS/Item Data/Melee Weapon", order = 1)]
	public class MeleeWeaponData : WeaponData, IMeleeWeaponData
	{
		[SerializeField] private MeleeWeaponType _meleeWeaponType;
		
		public MeleeWeaponType MeleeWeaponType => _meleeWeaponType;
		public override WeaponType WeaponType => WeaponType.Melee;

		public override bool TryResolveModel(ItemInventoryModels items, RangeWeaponInventoryModels range,
			MeleeWeaponInventoryModels melee, AmmoInventoryModels ammo, out InventoryItemModel model)
			=> melee.TryGetModel(_meleeWeaponType, out model);

		public override bool TryAddTo(PlayerInventoryWeapons weapons, PlayerInventoryItems items,
			PlayerInventoryAmmo ammo, out PlayerInventorySlot slot)
			=> weapons.TryAdd(this, out slot);

		public override bool TryRemoveFrom(PlayerInventoryWeapons weapons, PlayerInventoryItems items,
			PlayerInventoryAmmo ammo)
			=> weapons.TryRemove(_meleeWeaponType);
	}
}