namespace FAS.Players.States
{
	public class RangeWeaponStow : RangeWeaponState
	{
		public override PlayerStates Key => PlayerStates.RangeWeaponStow;
		
		public override bool IsPlayerControlledState => false;
		
		public override void Enter()
		{
			FollowCameraRotator.SnapBack();
			CamerasEnabler.DisableRangeWeaponAimCamera();
		}

		public override void Perform()
		{
			var weaponLayer = Animator.WeaponLayer;
			
			if (weaponLayer.CurrentAnimHash == weaponLayer.StowSniperRifleAnimHash && weaponLayer.CurrentAnimNTime > 0.75f)
				RequestTransition(PlayerStates.Free);
		}

		public override void Exit()
		{
			CameraBlendsChanger.TryReturnLastBlends();
			GamePause.TryPlay();
			base.Exit();
		}
	}
}