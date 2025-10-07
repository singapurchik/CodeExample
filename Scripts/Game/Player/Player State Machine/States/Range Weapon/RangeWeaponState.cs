using Zenject;

namespace FAS.Players.States
{
	public abstract class RangeWeaponState : PlayerState
	{
		[Inject] protected IRangeWeaponAimingScreenGroup ScreenGroup;
		[Inject] protected RangeWeaponTargetFinder TargetFinder;
		[Inject] protected IPlayerRangeWeapon Weapon;
	}
}