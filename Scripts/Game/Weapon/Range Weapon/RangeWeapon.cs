using UnityEngine;

namespace FAS
{
	public class RangeWeapon : Weapon<RangeWeaponData, CostumeRangeWeaponPoints>, IRangeWeaponInfo
	{
		[SerializeField] private int _startAmmoAmount;

		public RangeWeaponType RangeWeaponType => Data.RangeWeaponType;
		
		public float Damage => Data.Damage;

		public int CurrentAmmo { get; private set; }
		public int MaxAmmo => Data.MaxAmmo;

		protected override void Awake()
		{
			base.Awake();

			if (_startAmmoAmount > 0)
				CurrentAmmo = Mathf.Min(_startAmmoAmount, Data.MaxAmmo);
		}
	}
}