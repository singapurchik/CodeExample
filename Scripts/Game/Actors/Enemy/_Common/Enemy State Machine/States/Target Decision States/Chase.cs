using UnityEngine;

namespace FAS.Actors.Emenies
{
	public class Chase : TargetDecisionState
	{
		[SerializeField] private AnimatorStateIntervalAction _animatorStateIntervalAction;
		[SerializeField] private float _changeLocomotionLerpSpeed = 4f;
		[SerializeField] private float _angularSpeed = 500f;
		[SerializeField] private float _stoppingDistance = 0.75f;

		public override EnemyStates Key => EnemyStates.Chase;
		
		private void Start()
		{
			_animatorStateIntervalAction.Initialize(
				Animator.UpperBodyAdditiveLayer, Animator.UpperBodyAdditiveLayer.RunAnimHash, 
				Animator.PlayRunUpperBodyAdditiveAnim, Animator.ResetRunUpperBodyAdditiveAnim);
		}
		
		public override void Enter()
		{
			Mover.SetStoppingDistance(_stoppingDistance);
			Rotator.SetAutoAngularSpeed(_angularSpeed);
		}
		
		public override void Perform()
		{
			_animatorStateIntervalAction.TryInvoke();
			base.Perform();
		}

		protected override void OnTargetDetected()
		{
			Mover.NavMeshMove(TargetDetector.TargetPosition);
			Animator.RequestChangeLocomotionLerpSpeed(_changeLocomotionLerpSpeed);
			Animator.RequestRunAnim();
			
			if (Mover.IsFinishMoveThisFrame)
				RequestTransitionToAggressiveState();
		}

		protected override void OnNoTargetDetected()
		{
			RequestTransition(EnemyStates.Idle);
		}

		public override void Exit()
		{
			_animatorStateIntervalAction.Reset();
			base.Exit();
		}
	}
}