using UnityEngine;

namespace FAS.Players.States.Interaction
{
	public class RotateToInteraction : PlayerState, IPrepareInteractionState
	{
		[SerializeField] private float _rotationSpeed = 180f;
		
		public override PlayerStates Key => PlayerStates.RotateToInteraction;

		public override bool IsPlayerControlledState => false;
		public bool IsReadyToInteract { get; private set; }
		
		public override void Enter()
		{
			InputControl.DisableMovementInput();
			UIScreensSwitcher.HideAll();
			IsReadyToInteract = false;
		}

		public override void Perform()
		{
			if(!Rotator.TryRotateToTargetHorizontal(Interactor.InteractablePosition, _rotationSpeed))
				IsReadyToInteract = true;
			
			Jump.UpdateGravity();
		}
		
		public override void Exit()
		{
			IsReadyToInteract = false;
			base.Exit();
		}
	}
}