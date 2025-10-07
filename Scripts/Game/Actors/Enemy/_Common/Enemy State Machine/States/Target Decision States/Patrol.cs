using UnityEngine;
using Zenject;

namespace FAS.Actors.Emenies
{
	public class Patrol : TargetDecisionState
	{
		[SerializeField] private AnimatorStateIntervalAction _animatorStateIntervalAction;
		[SerializeField] private float _angularSpeed = 50f;
		
		[Inject] private EnemyPatrolPoints _points;
		
		public override EnemyStates Key => EnemyStates.Patrol;
		
		private void Start()
		{
			_animatorStateIntervalAction.Initialize(
				Animator.UpperBodyAdditiveLayer, Animator.UpperBodyAdditiveLayer.WalkAnimHash,
				Animator.PlayWalkUpperBodyAdditiveAnim, Animator.ResetWalkUpperBodyAdditiveAnim);
		}
		
		public override void Enter()
		{
			Mover.SetStoppingDistance(0);
			Mover.NavMeshMove(_points.Next.Position);
			Rotator.SetAutoAngularSpeed(_angularSpeed);
		}

		public override void Perform()
		{
			_animatorStateIntervalAction.TryInvoke();
			Animator.RequestWalkAnim();
			base.Perform();
		}

		protected override void OnNoTargetDetected()
		{
			if (!Mover.IsProcessMovement && Mover.IsMovedLastFrame)
			{
				_points.SetPointReached();
				RequestTransition(EnemyStates.Idle);
			}
		}

		public override void Exit()
		{
			_animatorStateIntervalAction.Reset();
			base.Exit();
		}
	}
}