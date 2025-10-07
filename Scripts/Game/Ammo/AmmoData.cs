using static System.String;
using FAS.Players;
using UnityEngine;
using FAS.Sounds;
using FAS.Items;

namespace FAS
{
	[CreateAssetMenu(fileName = "Ammo", menuName = "FAS/Item Data/Ammo", order = 0)]
	public class AmmoData : BaseItemData, IAmmoData
	{
		[SerializeField] private AmmoType _ammoType;

		public override SoundEvent InteractionUseSound => null;
		
		public override ItemType Type => ItemType.Ammo;
		public AmmoType AmmoType => _ammoType;
		
		public override string BuildedInteractionUseText => Empty;
		public override bool IsCanEquipFromInventory => false;
		public override bool IsCanUseFromInventory => false;
		public override bool IsUsingInInteraction => false;
		public override bool IsStackable => true;

		public override bool TryResolveModel(ItemInventoryModels items, RangeWeaponInventoryModels range,
			MeleeWeaponInventoryModels melee, AmmoInventoryModels ammo, out InventoryItemModel model)
			=> ammo.TryGetModel(AmmoType, out model);

		public override bool TryAddTo(PlayerInventoryWeapons weapons, PlayerInventoryItems items,
			PlayerInventoryAmmo ammo, out PlayerInventorySlot slot)
			=> ammo.TryAdd(this, out slot);

		public override bool TryRemoveFrom(PlayerInventoryWeapons weapons, PlayerInventoryItems items,
			PlayerInventoryAmmo ammo)
			=> ammo.TryRemove(_ammoType);
	}
}