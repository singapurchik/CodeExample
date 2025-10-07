using Cinemachine;
using UnityEngine;

namespace FAS.Players.States
{
	public class RangeWeaponReady : RangeWeaponState
	{
		[SerializeField] private CinemachineBlenderSettings _cameraBlends;
		
		public override PlayerStates Key => PlayerStates.RangeWeaponReady;

		public override bool IsPlayerControlledState => false;
		
		public override void Enter()
		{
			GamePause.TryPause(Pausable);
			CameraBlendsChanger.TryChangeCameraBlends(_cameraBlends);
			UIScreensSwitcher.HideAll();
			
			switch (Weapon.Info.RangeWeaponType)
			{
				default:
				case RangeWeaponType.Pistol:
					break;
				case RangeWeaponType.Shotgun:
					break;
				case RangeWeaponType.SniperRifle:
					Animator.PlaySniperRifleAiming();
					break;
			}
		}

		public override void Perform()
		{
			var weaponLayer = Animator.WeaponLayer;
			
			if (weaponLayer.CurrentAnimHash == weaponLayer.ReadySniperRifleAnimHash && weaponLayer.CurrentAnimNTime > 0.5f)
				RequestTransition(PlayerStates.RangeWeaponAiming);
		}

		public override void Exit()
		{
			CamerasEnabler.EnableRangeWeaponAimCamera();
			base.Exit();
		}
	}
}