namespace FAS.Players
{
	public interface IPlayerRangeWeapon
	{
		public IRangeWeaponInfo Info { get; }
		
		public void Shoot();
	}
}