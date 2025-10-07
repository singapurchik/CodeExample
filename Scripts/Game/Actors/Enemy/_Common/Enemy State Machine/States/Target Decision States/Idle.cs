using UnityEngine;

namespace FAS.Actors.Emenies
{
	public class Idle : TargetDecisionState
	{
		[SerializeField] private AnimatorStateIntervalAction _animatorStateIntervalAction;
		[SerializeField] private float _idleTimeAfterAttack = 2f;
		[SerializeField] private float _rotationSpeed = 180f;
		
		private float _uninterruptibleIdleTime;
		private float _currentIdleTime;

		public override EnemyStates Key => EnemyStates.Idle;

		private void Start()
		{
			_animatorStateIntervalAction.Initialize(
				Animator.UpperBodyAdditiveLayer, Animator.UpperBodyAdditiveLayer.IdleAnimHash,
				Animator.PlayIdleUpperBodyAdditiveAnim, Animator.ResetIdleUpperBodyAdditiveAnim);
		}

		public override void Enter()
		{
			var currentPoint = PointsHolder.Current;
			Mover.TryStopMove();
			Animator.PlayIdleUpperBodyAdditiveAnim();
			_uninterruptibleIdleTime = 0;

			if (StateMachine.TryGetLastStateKey(out var key) && key == EnemyStates.Attack)
				_uninterruptibleIdleTime = Time.timeSinceLevelLoad + _idleTimeAfterAttack;
			else
				_currentIdleTime = Time.timeSinceLevelLoad + currentPoint.IdleTime;
		}

		protected override bool IsTransitionBlocked() => Time.timeSinceLevelLoad < _uninterruptibleIdleTime;
		
		public override void Perform()
		{
			_animatorStateIntervalAction.TryInvoke();
			Mover.RequestEnableRootMotion();
			base.Perform();
		}

		protected override void OnTargetDetected()
		{
			base.OnTargetDetected();
			RotateToTarget();
		}

		protected override void OnNoTargetDetected()
		{
			Rotator.RequestRotateHorizontal(PointsHolder.Current.HorizontalRotation, _rotationSpeed);
			
			if (StateMachine.IsUsePatrolState && Time.timeSinceLevelLoad > _currentIdleTime)
				RequestTransition(EnemyStates.Patrol);
		}

		public override void Exit()
		{
			_animatorStateIntervalAction.Reset();
			base.Exit();
		}
	}
}