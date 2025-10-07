using UnityEngine;

namespace FAS.Actors.Companion
{
	public class StandUpFromBed : CompanionState
	{
		[SerializeField] private float _standUpOffset = 0.265f;
		[SerializeField] private float _moveSpeed = 10f;

		private float _standUpHeight;
		
		public override GirlStates Key =>  GirlStates.StandUpFromBed;
		
		private const float ANIM_N_TIME_FOR_TRANSITION_TO_IDLE = 0.8f;
		private const float ANIM_N_TIME_FOR_CHANGE_HEIGHT = 0.62f;
		
		public override void Enter()
		{
			_standUpHeight = transform.parent.localPosition.y + _standUpOffset;
			Animator.PlayStandUpFromBedAnim();
		}

		public override void Perform()
		{
			Mover.RequestDisableNavMeshAgent();
			Mover.RequestEnableRootMotion();
			
			var animInfo = Animator.UniqueStateLayer;
			
			if (animInfo.CurrentAnimHash == animInfo.StandUpFromBedAnimHash
			    && animInfo.CurrentAnimNTime > ANIM_N_TIME_FOR_CHANGE_HEIGHT)
			{
				var targetPosition = transform.parent.localPosition;
				targetPosition.y = _standUpHeight;
				transform.parent.localPosition = Vector3.MoveTowards(transform.parent.localPosition,
					targetPosition, _moveSpeed * Time.deltaTime);
				
				if (Mathf.Abs(transform.parent.localPosition.y - _standUpHeight) == 0
				    && animInfo.CurrentAnimNTime > ANIM_N_TIME_FOR_TRANSITION_TO_IDLE)
						RequestTransition(GirlStates.Idle);
			}
		}
	}
}