using FAS.Fatality;
using UnityEngine;
using Zenject;

namespace FAS.Players.States
{
	public class FatalityReceive : PlayerState
	{
		[SerializeField] private float _rotationSpeed = 720f;
		[SerializeField] private float _moveSpeed = 3f;
		
		[Inject] private FatalityTarget _fatalityTarget;
		
		public override PlayerStates Key => PlayerStates.FatalityReceive;
		
		public override bool IsPlayerControlledState => false;

		public override void Enter()
		{
			_fatalityTarget.OnPerformFatality += PlayFatalityAnim;
			AnimEvents.OnKnifeOutOfStomach += OnKnifeOutOfStomach;
			AnimEvents.OnLeftLegSlashed += LeftLegSlashed;
			AnimEvents.OnStabStomach += OnStabStomach;
			AnimEvents.OnNeckSlashed += OnNeckSlashed;

			CamerasEnabler.EnableFatalityCamera(FatalityTarget.CurrentData.CameraPoint);
			UIScreensSwitcher.HideAll();
			InputControl.DisableMovementInput();
			InputControl.DisableButtonsInput();
			_fatalityTarget.SetReadyToFatality();
		}

		private void OnKnifeOutOfStomach()
		{
			CameraShaker.PlayKnifeOutOfStomach();
			VisualEffects.PlayKnifeOutOfStomachBloodEffect();
			SoundEffects.PlayKnifeOutOfStomach();
		}

		private void OnStabStomach()
		{
			CameraShaker.PlayStabStomach();
			VisualEffects.PlayStabStomachBloodEffect();
			SoundEffects.PlayStabStomach();
		}
		
		private void OnNeckSlashed()
		{
			CameraShaker.PlayNeckSlash();
			VisualEffects.PlayNeckSlashBloodEffect();
			SoundEffects.PlayNeckSlash();
		}

		private void LeftLegSlashed()
		{
			CameraShaker.PlayLeftLegSlash();
			VisualEffects.PlayLeftLagSlashBloodEffect();
			SoundEffects.PlayLeftLegSlash();
		}
		
		public override void Perform()
		{
			Mover.RequestTransformMove(FatalityTarget.CurrentData.Position, _moveSpeed);
			Rotator.SmoothRotateHorizontal(FatalityTarget.CurrentData.RotationAngles.y, _rotationSpeed);

			if (Animator.FatalityLayer.IsActive && Animator.FatalityLayer.CurrentAnimNTime >= 1)
			{
				_fatalityTarget.SetFatalityCompletedState(true);
				RequestTransition(PlayerStates.Death);
			}
		}
		
		private void PlayFatalityAnim()
		{
			Animator.PlayFatalityAnim(FatalityTarget.CurrentData.Type);
		}

		public override void Exit()
		{
			AnimEvents.OnNeckSlashed -= OnNeckSlashed;
			AnimEvents.OnStabStomach -= OnStabStomach;
			AnimEvents.OnLeftLegSlashed -= LeftLegSlashed;
			AnimEvents.OnKnifeOutOfStomach -= OnKnifeOutOfStomach;
			_fatalityTarget.OnPerformFatality -= PlayFatalityAnim;
			base.Exit();
		}
	}
}