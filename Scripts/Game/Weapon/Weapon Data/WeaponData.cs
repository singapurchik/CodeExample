using static System.String;
using UnityEngine;
using FAS.Sounds;
using FAS.Items;

namespace FAS
{
	public abstract class WeaponData : BaseItemData, IWeaponData
	{
		[SerializeField] private float _damage = 1;

		public override ItemType Type => ItemType.Weapon;
		public abstract WeaponType WeaponType { get; }

		public override SoundEvent InteractionUseSound => null;

		public float Damage => _damage;

		public override string BuildedInteractionUseText => Empty;

		public override bool IsCanEquipFromInventory => true;
		public override bool IsCanUseFromInventory => false;
		public override bool IsUsingInInteraction => false;
		public override bool IsStackable => false;
		
		public void SetEquipped(bool isEquipped) => IsEquipped = isEquipped;
	}
}