using UnityEngine;

namespace FAS.Players.States.Interaction
{
	public class MoveToInteraction : PlayerState, IPrepareInteractionState
	{
		[SerializeField] private float _rotateToInteractionSpeed = 180f;
		[SerializeField] private float _moveToInteractionSpeed = 3f;
		
		public override PlayerStates Key => PlayerStates.MoveToInteraction;
		
		private const float DISTANCE_TO_INTERACT = 0.1f;
		private const float ANGLE_TO_INTERACT = 0.1f;
		private const float MAX_LOCOMOTION_VALUE = 1f;

		private bool _isMoving;
		
		public override bool IsPlayerControlledState => false;

		public bool IsReadyToInteract { get; private set; }
		
		public override void Enter()
		{
			InputControl.DisableMovementInput();
			UIScreensSwitcher.HideAll();
			_isMoving = true;
			IsReadyToInteract = false;
		}
		
		private void Move(float sqrMagnitude)
		{
			Mover.RequestTransformMove(Interactor.InteractablePosition, _moveToInteractionSpeed);
			
			var sqrMagnitudeToLocomotionValue = Mathf.Min(Mathf.Sqrt(sqrMagnitude), MAX_LOCOMOTION_VALUE);
			Animator.ForceSetLocomotionValue(sqrMagnitudeToLocomotionValue);
			
			if (Vector3.SqrMagnitude(transform.position - Interactor.InteractablePosition) <= DISTANCE_TO_INTERACT)
				_isMoving = false;
		}
		
		private void RotateToPointDirection(Vector3 direction)
			=> Rotator.SmoothRotateToDirection(direction, _rotateToInteractionSpeed);

		private void RotateToPointAngles()
			=> Rotator.SmoothRotateHorizontal(Interactor.InteractableRotationAngles.y, _rotateToInteractionSpeed);
		
		private bool IsLookAtInteractable()
			=> Mathf.Abs(
				Mathf.DeltaAngle(transform.eulerAngles.y, 
					Interactor.InteractableRotationAngles.y)) < ANGLE_TO_INTERACT;

		public override void Perform()
		{
			var sqrMagnitude =
				Vector3.SqrMagnitude(transform.position - Interactor.InteractablePosition);

			Move(sqrMagnitude);

			if (_isMoving && sqrMagnitude > DISTANCE_TO_INTERACT)
			{
				var targetPosition = Interactor.InteractablePosition;
				targetPosition.y = transform.position.y;
				RotateToPointDirection(targetPosition - transform.position);
			}
			else if (!IsLookAtInteractable())
			{
				RotateToPointAngles();
			}
			else if (!Animator.BaseLayer.IsInTransition)
			{
				IsReadyToInteract = true;
			}

			Jump.UpdateGravity();
		}

		public override void Exit()
		{
			IsReadyToInteract = false;
			Animator.ForceSetLocomotionValue(0);
			Mover.RequestTeleportTo(Interactor.InteractablePosition);
			Rotator.ForceRotateHorizontal(Interactor.InteractableRotationAngles.y);
			base.Exit();
		}
	}
}