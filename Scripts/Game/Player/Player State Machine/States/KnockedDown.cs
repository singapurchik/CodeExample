using UnityEngine;

namespace FAS.Players.States
{
	public class KnockedDown : PlayerState
	{
		[SerializeField] private Vector3 _characterControllerCenterOffset = new (0, 0, -0.5f);

		private bool _isWaitingOneFrame;
		
		public override PlayerStates Key => PlayerStates.KnockedDown;
		
		public override bool IsPlayerControlledState => false;
		
		public override void Enter()
		{
			SoundEffects.PlayKnockDownAndStandUp();
			InputControl.DisableMovementInput();
			UIScreensSwitcher.HideAll();
			Animator.PlayKnockedDownAnim();
			_isWaitingOneFrame = true;
		}
		
		public override void Perform()
		{
			CharacterController.RequestAddCenterOffset(_characterControllerCenterOffset);

			if (_isWaitingOneFrame)
			{
				_isWaitingOneFrame = false;
			}
			else 
			{
				var animLayer = Animator.FullBodyReactionLayer;
				
				if (animLayer.IsEnabled && Animator.IsCrouched())
					Animator.EnableStandLocomotion();
					
				if (animLayer.CurrentAnimHash == Layer.EmptyAnimHash)
					StateReturner.TryReturnLastControlledState();
			}
			
			Jump.UpdateGravity();
		}

		public override void Exit()
		{
			UIScreensSwitcher.TryOpenCurrent();
			base.Exit();
		}
	}
}