using FAS.Fatality;
using UnityEngine;
using Zenject;

namespace FAS.Actors.Emenies
{
	public enum EnemyStates
	{
		Undefined = 0,
		Idle,
		Chase,
		Patrol,
		Attack,
		ShowingFace,
		FatalityExecute,
	}
	
	public abstract class EnemyState : State<EnemyStates, EnemyStates>
	{
		[Inject] protected ICinemachineBrainInfo CinemachineBrainInfo;
		[Inject] protected IEnemyStateMachineInfo StateMachine;
		[Inject] protected IEnemyPatrolPointHolder PointsHolder;
		[Inject] protected EnemyAnimEventsReceiver AnimEvents;
		[Inject] protected EnemySoundEffects SoundEffects;
		[Inject] protected IFatalityTarget FatalityTarget;
		[Inject] protected TargetDetector TargetDetector;
		[Inject] protected EnemyAnimator Animator;
		[Inject] protected EnemyRotator Rotator;
		[Inject] protected EnemyMover Mover;

		protected bool IsTargetNear(float minDistance)
		{
			if (TargetDetector.IsTargetDetected)
			{
				var targetPosition = TargetDetector.TargetPosition;
				targetPosition.y = transform.position.y;
				return !(Vector3.SqrMagnitude(transform.position - targetPosition) > minDistance * minDistance);
			}
			return false;
		}

		public void RotateToTarget()
		{
			var direction = TargetDetector.TargetPosition - transform.position;
			direction.y = 0;
			Rotator.RequestRotateHorizontal(Quaternion.LookRotation(direction).eulerAngles.y);
		}
		
		protected virtual bool IsTransitionBlocked() => false;
	}
}
