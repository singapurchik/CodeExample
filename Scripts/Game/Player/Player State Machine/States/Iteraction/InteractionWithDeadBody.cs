using UnityEngine;

namespace FAS.Players.States.Interaction
{
	public class InteractionWithDeadBody : PlayerState
	{
		[SerializeField] private float _wakeUpBodyAnimNTimeForKnockedDown = 0.75f;
		
		private DeadBody _deadBody;
		
		public override PlayerStates Key => PlayerStates.InteractionWithDeadBody;

		public override bool IsPlayerControlledState => false;
		
		public void SetDeadBody(DeadBody deadBody) => _deadBody = deadBody;
		
		public override void Enter()
		{
			InputControl.DisableMovementInput();
			UIScreensSwitcher.HideAll();
			_deadBody.PlayWakeUpAnim();
			SoundEffects.PlayJumpscare(1);
		}

		public override void Perform()
		{
			if (_deadBody.IsWakeUpAnimPlaying && _deadBody.AnimNormalizedTime > _wakeUpBodyAnimNTimeForKnockedDown)
				RequestTransition(PlayerStates.KnockedDown);
		}
	}
}