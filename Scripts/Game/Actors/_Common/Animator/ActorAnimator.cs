using UnityEngine;

namespace FAS.Actors
{
	public abstract class ActorAnimator : CharacterAnimator
	{
		[SerializeField] private float _idleAnimSpeedMultiplier = 1f;
		[SerializeField] private float _walkAnimSpeedMultiplier = 1f;
		[SerializeField] private float _runAnimSpeedMultiplier = 1f;
		
		protected float RequestedChangeLocomotionLerpSpeed;
		protected float CurrentChangeLocomotionLerpSpeed;
		protected float CurrentLocomotionSpeedMultiplier;
		protected float RequestedSetLocomotionValue;

		protected bool IsChangeLocomotionLerpSpeedRequested;
		protected bool IsChangeLocomotionValueRequested;
		
		public Quaternion DeltaRotation => Animator.deltaRotation;
		public Vector3 DeltaPosition => Animator.deltaPosition;
		public Vector3 Velocity => Animator.velocity;
		
		private readonly int _locomotionSpeedMultiplier = Animator.StringToHash("locomotionSpeedMultiplier");

		protected const float DEFAULT_LOCOMOTION_LERP_SPEED = 1f;
		protected const float IDLE_LOCOMOTION_VALUE = 0f;
		protected const float WALK_LOCOMOTION_VALUE = 0.5f;
		protected const float RUN_LOCOMOTION_VALUE = 1f;

		protected override void Awake()
		{
			base.Awake();
			CurrentLocomotionSpeedMultiplier = Animator.GetFloat(_locomotionSpeedMultiplier);
		}

		public void RequestChangeLocomotionLerpSpeed(float speed)
		{
			RequestedChangeLocomotionLerpSpeed = speed;
			IsChangeLocomotionLerpSpeedRequested = true;
		}

		public void RequestWalkAnim() => RequestSetLocomotion(WALK_LOCOMOTION_VALUE);

		public void RequestRunAnim() => RequestSetLocomotion(RUN_LOCOMOTION_VALUE);
		
		public void Restart() => Animator.Rebind();

		private void RequestSetLocomotion(float normalizedValue)
		{
			RequestedSetLocomotionValue = normalizedValue;
			IsChangeLocomotionValueRequested = true;
		}

		protected void UpdateLocomotionSpeedMultiplier(float oldLocomotionValue, float targetLocomotionValue,
			float currentLocomotionValue)
		{
			var targetMultiplier = targetLocomotionValue switch
			{
				IDLE_LOCOMOTION_VALUE => _idleAnimSpeedMultiplier,
				WALK_LOCOMOTION_VALUE => _walkAnimSpeedMultiplier,
				RUN_LOCOMOTION_VALUE => _runAnimSpeedMultiplier,
				_ => 1f
			};

			CurrentLocomotionSpeedMultiplier = Mathf.Lerp(CurrentLocomotionSpeedMultiplier, targetMultiplier,
				Mathf.InverseLerp(oldLocomotionValue, targetLocomotionValue, currentLocomotionValue));
			
			Animator.SetFloat(_locomotionSpeedMultiplier, CurrentLocomotionSpeedMultiplier);	
		}

		protected void AutoLayerWeightControl(Layer layer) => AutoLayerWeightControl(layer, layer.IsActive);
		
		protected void AutoLayerWeightControl(Layer layer, bool isActive)
		{
			if (isActive)
			{
				if (!layer.IsEnabled)
					layer.EnableWeightSmooth();
			}
			else if (!layer.IsDisabled)
			{
				layer.DisableWeightSmooth();
			}
		}
	}
}