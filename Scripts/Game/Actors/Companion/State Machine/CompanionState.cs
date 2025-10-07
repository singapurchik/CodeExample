using FAS.Actors.Companion.Animations;
using UnityEngine;
using Zenject;

namespace FAS.Actors.Companion
{
	public enum GirlStates
	{
		Undefined = 0,
		StandUpFromBed = 1,
		Idle = 2,
		FollowPlayer = 3,
		KnockedBack = 4,
		Teleport = 5,
		ChillingOnBed = 6
	}
	
	public abstract class CompanionState : State<GirlStates, GirlStates>
	{
		[Inject] protected CompanionAnimationRigging AnimationRigging;
		[Inject] protected CompanionAnimator Animator;
		[Inject] protected CompanionRotator Rotator;
		[Inject] protected CompanionMover Mover;
		[Inject] protected ICompanion Companion;

		protected Vector3 GetHorizontalDirectionToOwner()
		{
			var direction = Companion.Owner.FollowPosition - transform.position;
			direction.y = 0f;
			return direction;
		}
		
		protected bool IsTargetNear(Vector3 target, float minDistance)
		{
			target.y = transform.position.y;
			return !(Vector3.SqrMagnitude(transform.position - target) > minDistance * minDistance);
		}
	}
}