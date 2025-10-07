namespace FAS
{
	public interface IRangeWeaponInfo
	{
		public RangeWeaponType RangeWeaponType { get; }
		
		public float Damage { get; }
		
		public int CurrentAmmo { get; }
		public int MaxAmmo { get; }
	}
}