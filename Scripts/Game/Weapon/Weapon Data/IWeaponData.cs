using FAS.Items;

namespace FAS
{
	public interface IWeaponData : IItemData
	{
		public WeaponType WeaponType { get; }
	}
}