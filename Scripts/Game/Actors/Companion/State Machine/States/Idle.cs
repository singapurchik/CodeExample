using UnityEngine;

namespace FAS.Actors.Companion
{
	public class Idle : CompanionState
	{
		[SerializeField] private float _minIdleTime = 1.5f;
		[SerializeField] private float _maxIdleDistance = 1f;
		[SerializeField] private float _rotateFullBodySpeed = 3f;
		[SerializeField] private float _checkDisplacementInterval = 0.25f;
		[SerializeField] private float _maxDisplacementDistance = 0.1f;

		private Vector3 _lastCheckDisplacementPosition;
		
		private float _nextCheckDisplacementTime;
		
		public override GirlStates Key => GirlStates.Idle;

		private const float ANGLE_FOR_FULL_BODY_ROTATION = 90f;
		private const float SPINE_ROTATION_OFFSET = 35f;

		public override void Enter()
		{
			_lastCheckDisplacementPosition = transform.position;
			_nextCheckDisplacementTime = Time.timeSinceLevelLoad + _checkDisplacementInterval;
			
			Mover.TryStopMove();
			Animator.PlayStartLocomotionAnim();
		}

		public override void Perform()
		{
			if (Vector3.SqrMagnitude(transform.position - _lastCheckDisplacementPosition)
			    > _maxDisplacementDistance * _maxDisplacementDistance)
					RequestTransition(GirlStates.KnockedBack);

			if (Time.timeSinceLevelLoad > _nextCheckDisplacementTime)
			{
				_lastCheckDisplacementPosition = transform.position;
				_nextCheckDisplacementTime = Time.timeSinceLevelLoad + _checkDisplacementInterval;
			}
			
			if (Settings.CurrentGraphicsQuality != GraphicsQualityType.Low)
			{
				var targetDirection = GetHorizontalDirectionToOwner();
				var angle = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);

				var isClockwise = angle > 0;
				var absAngle = Mathf.Abs(angle);
				var spineAngle = 0f;
				
				if (absAngle > SPINE_ROTATION_OFFSET)
				{
					if (isClockwise)
						spineAngle = angle - SPINE_ROTATION_OFFSET;
					else
						spineAngle = angle + SPINE_ROTATION_OFFSET;
				}
				
				AnimationRigging.RequestEnabledSpineRig(spineAngle);
				AnimationRigging.RequestEnabledHeadRig(angle);
				
				if (Animator.BaseLayer.IsIdleAnimPlaying)
					Mover.RequestEnableRootMotion();

				if (Animator.LowerBodyLayer.IsActive || Animator.LowerBodyLayer.IsInTransition)
				{
					Rotator.RequestRotateHorizontal(
						Quaternion.LookRotation(targetDirection).eulerAngles.y, _rotateFullBodySpeed);
				}
				else if (absAngle > ANGLE_FOR_FULL_BODY_ROTATION)
				{
					if (isClockwise)
						Animator.RequestTurnRight();
					else
						Animator.RequestTurnLeft();
				}	
			}
			
			if (!IsTargetNear(Companion.Owner.FollowPosition, _maxIdleDistance))
				RequestTransition(GirlStates.FollowPlayer);	
		}
	}
}