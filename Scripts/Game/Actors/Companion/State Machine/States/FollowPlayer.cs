using UnityEngine;

namespace FAS.Actors.Companion
{
	public class FollowPlayer : CompanionState
	{
		[SerializeField] private float _walkDistance = 2f;
		[SerializeField] private float _stoppingDistance = 0.5f;
		[SerializeField] private float _angularSpeed = 720;
		[SerializeField] private float _followDistance = 3f;
		[SerializeField] private float _distanceToTeleport = 20f;
		
		private Coroutine _teleportCoroutine;
		
		public override GirlStates Key => GirlStates.FollowPlayer;

		public override void Enter()
		{
			Mover.SetStoppingDistance(_stoppingDistance);
			Rotator.SetAutoAngularSpeed(_angularSpeed);
		}

		private void RunFollowPlayer()
		{
			Mover.NavMeshMove(Companion.Owner.FollowPosition);
			Animator.RequestRunAnim();
		}

		private void WalkFollowPlayer()
		{
			Mover.NavMeshMove(Companion.Owner.FollowPosition);
			Animator.RequestWalkAnim();
		}

		public override void Perform()
		{
			var sqrMagnitudeToPlayer = Vector3.SqrMagnitude(transform.position - Companion.Owner.FollowPosition);

			if (sqrMagnitudeToPlayer > _distanceToTeleport * _distanceToTeleport)
			{
				RequestTransition(GirlStates.Teleport);
			}
			else
			{
				if (sqrMagnitudeToPlayer > _walkDistance * _walkDistance)
					RunFollowPlayer();
				else
					WalkFollowPlayer();
			
				if (Mover.IsFinishMoveThisFrame)
					RequestTransition(GirlStates.Idle);	
			}
		}
	}
}