using UnityEngine;
using System;
using Zenject;

namespace FAS.Players.Animations
{
	public class PlayerAnimEventsReceiver : MonoBehaviour
	{
		[Inject] private PlayerAnimator _animator;

		public event Action<float> OnSetWeaponToRightHand;
		public event Action<float> OnSetWeaponToLeftHand;
		public event Action<float> OnSetWeaponToSpine;
		public event Action OnKnifeOutOfStomach;
		public event Action OnLeftLegSlashed;
		public event Action OnFootstepWalk;
		public event Action OnNeckSlashed;
		public event Action OnStabStomach;
		public event Action OnFootstepRun;
		public event Action OnShoot;

		private void AE_SetWeaponToRightHand(float duration) => OnSetWeaponToRightHand?.Invoke(duration);
		
		private void AE_SetWeaponToLeftHand(float duration) => OnSetWeaponToLeftHand?.Invoke(duration);
		
		private void AE_SetWeaponToSpine(float duration) => OnSetWeaponToSpine?.Invoke(duration);
		
		private void AE_KnifeOutOfStomach() => OnKnifeOutOfStomach?.Invoke();
		
		private void AE_LeftLegSlashed() => OnLeftLegSlashed?.Invoke();
		
		private void AE_NeckSlashed() => OnNeckSlashed?.Invoke();
		
		private void AE_StabStomach() => OnStabStomach?.Invoke();
		
		private void AE_OnShoot() => OnShoot?.Invoke();
		
		private void AE_FootStepWalk()
		{
			if (_animator.FatalityLayer.Weight < 0.1f)
				OnFootstepWalk?.Invoke();
		}
		
		private void AE_FootStepRun()
		{
			if (_animator.FatalityLayer.Weight < 0.1f)
				OnFootstepRun?.Invoke();
		}
	}
}