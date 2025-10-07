namespace FAS.Players.States
{
	public class RangeWeaponShooting : RangeWeaponState
	{
		public override PlayerStates Key => PlayerStates.RangeWeaponShooting;

		public override bool IsPlayerControlledState => false;

		private int _maxShootCount;
		private int _shootCount;
		
		public override void Enter()
		{
			_maxShootCount = Weapon.Info.CurrentAmmo;
			Shoot(true);
		}

		private void Shoot(bool isUseBlending)
		{
			Animator.PlayShootAnim(isUseBlending);
			_shootCount++;
		}

		public override void Perform()
		{
			var weaponLayer = Animator.WeaponLayer;
			
			if (!weaponLayer.IsInTransition
			    && weaponLayer.CurrentAnimHash == weaponLayer.ShootSniperRifleAnim
			    && weaponLayer.CurrentAnimNTime > 0.99f)
			{
				// if (_shootCount < _maxShootCount)
				// 	Shoot(false);
				RequestTransition(PlayerStates.RangeWeaponAiming);
			}
		}

		public override void Exit()
		{
			Animator.StopShootingAnim();
			base.Exit();
		}
	}
}