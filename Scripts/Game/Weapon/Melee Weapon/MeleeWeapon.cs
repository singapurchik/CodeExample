namespace FAS
{
	public class MeleeWeapon : Weapon<MeleeWeaponData, CostumeMeleeWeaponPoints>, IMeleeWeaponInfo
	{
		public MeleeWeaponType MeleeWeaponType => Data.MeleeWeaponType;
	}
}