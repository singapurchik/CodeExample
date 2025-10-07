namespace FAS.Players.States
{
	public class RangeWeaponReloading : RangeWeaponState
	{
		public override PlayerStates Key => PlayerStates.RangeWeaponReloading;

		public override bool IsPlayerControlledState => false;
		
		public override void Enter()
		{
		}
	}
}