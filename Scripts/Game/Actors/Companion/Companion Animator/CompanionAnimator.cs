using UnityEngine;
using Zenject;

namespace FAS.Actors.Companion.Animations
{
	public class CompanionAnimator : ActorAnimator
	{
		[Inject] private UniqueStateLayer _uniqueStateLayer;
		[Inject] private LowerBodyLayer _lowerBodyLayer;
		[Inject] private IPausableInfo _pausable;
		[Inject] private BaseLayer _baseLayer;
		
		public IReadOnlyUniqueStateLayer UniqueStateLayer => _uniqueStateLayer;
		public IReadOnlyAnimatorLayer LowerBodyLayer => _lowerBodyLayer;
		public IReadOnlyBaseLayer BaseLayer => _baseLayer;
		
		public void PlayStandUpFromBedAnim() => _uniqueStateLayer.PlayStandUpFromBedAnim();
		
		public void PlayChillingOnBedAnim() => _uniqueStateLayer.PlayChillingOnBedAnim();
		
		public void PlayStartLocomotionAnim() => _baseLayer.PlayStartLocomotionAnim();

		public void RequestTurnRight() => _lowerBodyLayer.PlayTurnInPlaceAnim(1);

		public void RequestTurnLeft() => _lowerBodyLayer.PlayTurnInPlaceAnim(0);

		public void PlayKnockedBackAnim() => _baseLayer.PlayKnockedBackAnim();
		
		public void ForcePlayIdleAnim()
		{
			RequestedSetLocomotionValue = IDLE_LOCOMOTION_VALUE;
			_baseLayer.SetLocomotionValue(IDLE_LOCOMOTION_VALUE);
		}
		
		private void TryChangeLocomotionValueSmooth(float normalizedValue)
		{
			if (IsForceUpdateThisFrame || _baseLayer.LocomotionValue != normalizedValue)
			{
				var targetLayerValue = Mathf.MoveTowards(_baseLayer.LocomotionValue, normalizedValue, 
					CurrentChangeLocomotionLerpSpeed * Time.deltaTime);
			
				UpdateLocomotionSpeedMultiplier(_baseLayer.LocomotionValue, normalizedValue, targetLayerValue);
				_baseLayer.SetLocomotionValue(targetLayerValue);
			}
		}
		
		private void Update()
		{
			if (_pausable.IsPaused) return;

			AutoLayerWeightControl(_uniqueStateLayer);
			AutoLayerWeightControl(_lowerBodyLayer);
			
			if (IsChangeLocomotionLerpSpeedRequested)
			{
				CurrentChangeLocomotionLerpSpeed = RequestedChangeLocomotionLerpSpeed;
				IsChangeLocomotionLerpSpeedRequested = false;
			}
			
			if (IsChangeLocomotionValueRequested)
			{
				TryChangeLocomotionValueSmooth(RequestedSetLocomotionValue);
				IsChangeLocomotionValueRequested = false;
			}
			else if (_baseLayer.LocomotionValue > 0)
			{
				TryChangeLocomotionValueSmooth(IDLE_LOCOMOTION_VALUE);
			}

			CurrentChangeLocomotionLerpSpeed = DEFAULT_LOCOMOTION_LERP_SPEED;
			IsForceUpdateThisFrame = false;
		}
	}
}