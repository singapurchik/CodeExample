using System.Collections.Generic;
using UnityEngine;

namespace FAS.Players.Animations
{
	public class WeaponLayer : Layer, IReadOnlyWeaponLayer
	{
		private static readonly int _readySniperRifleAnimHash = Animator.StringToHash("Ready Sniper Rifle");
		private static readonly int _aimingSniperRifleAnimHash = Animator.StringToHash("Aiming Sniper Rifle");
		private static readonly int _shootSniperRifleAnimHash = Animator.StringToHash("Shoot Sniper Rifle");
		private static readonly int _stowSniperRifleAnimHash = Animator.StringToHash("Stow Sniper Rifle");
		private static readonly int _isShootingBoolHash = Animator.StringToHash("isShooting");
		private static readonly int _isHasAmmoBoolHash = Animator.StringToHash("isHasAmmo");
		private static readonly int _isAimingBoolHash = Animator.StringToHash("isAiming");

		public int AimingSniperRifleAnimHash => _aimingSniperRifleAnimHash;
		public int ReadySniperRifleAnimHash => _readySniperRifleAnimHash;
		public int StowSniperRifleAnimHash => _stowSniperRifleAnimHash;
		public int ShootSniperRifleAnim => _shootSniperRifleAnimHash;

		public WeaponLayer(Animator animator, int index, List<Layer> layersList) : base(animator, index, layersList)
		{
		}
		
		public void SetIsHasAmmoState(bool value) => Animator.SetBool(_isHasAmmoBoolHash, value);

		public void PlayFinishAiming() => Animator.SetBool(_isAimingBoolHash, false);
		
		public void PlayStartAiming() => Animator.SetBool(_isAimingBoolHash, true);
		
		public void PlayReadySniperRifle() => PlayAnim(ReadySniperRifleAnimHash);

		public void PlayShootingAnim(bool isUseBlending)
		{
			if (isUseBlending)
				Animator.SetBool(_isShootingBoolHash, true);
			else
				PlayAnim(_shootSniperRifleAnimHash);
		}

		public void StopShootingAnim()
		{
			Animator.SetBool(_isShootingBoolHash, false);
		}
	}
}