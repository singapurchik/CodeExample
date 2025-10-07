using UnityEngine;

namespace FAS.Players.States
{
	public class Free : PlayerState
	{
		[SerializeField] private float _rotationSpeed = 20f;
		
		public override PlayerStates Key => PlayerStates.Free;
		
		public override bool IsPlayerControlledState => true;

		public override void Enter()
		{
			UIScreensSwitcher.ShowGameplayScreen();
			InputControl.EnableMovementInput();
			Animator.EnableStandLocomotion();
		}
		
		public override void Perform()
		{
			Animator.RequestSetLocomotionValue(Input.GetJoystickDirection2D().magnitude);
			Rotator.SmoothRotateToDirection(Input.GetJoystickDirection3D(), _rotationSpeed);
			InteractableFinderUpdater.RequestUpdate();
			Jump.UpdateGravity();
		}
	}
}