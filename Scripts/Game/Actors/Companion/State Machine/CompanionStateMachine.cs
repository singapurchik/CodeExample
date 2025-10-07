using System;
using FAS.Actors.Companion.Animations;
using Zenject;

namespace FAS.Actors.Companion
{
	public class CompanionStateMachine :  StateMachine<GirlStates, CompanionState>
	{
		[Inject] private CompanionAnimator _animator;
		
		[Inject] private TeleportToPlayer _teleportToPlayerState;
		[Inject] private ChillingOnBed _chillingOnBedState;
		[Inject] private StandUpFromBed _standUpFromBedState;
		[Inject] private FollowPlayer _followPlayerState;
		[Inject] private KnockedBack _knockedBackState;
		[Inject] private Idle _idleState;
		
		private void Awake()
		{
			AddState(_teleportToPlayerState);
			AddState(_chillingOnBedState);
			AddState(_standUpFromBedState);
			AddState(_followPlayerState);
			AddState(_knockedBackState);
			AddState(_idleState);
		}

		public override void Initialize() => SwitchStateTo(_chillingOnBedState);
		
		public void TrySwitchStateToStandUpFromBed() => TrySwitchStateTo(_standUpFromBedState);

		protected override void ExitCurrentState()
		{
			base.ExitCurrentState();
			_animator.TryResetTriggers();
		}
	}
}