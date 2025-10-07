using FAS.Actors.Emenies.Animations;
using FAS.Fatality;
using UnityEngine;
using Zenject;

namespace FAS.Actors.Emenies
{
	public class EnemyAnimator : ActorAnimator
	{
		[SerializeField] private bool _isUseUpperBodyAdditiveLayer;
		
		[Inject] private UpperBodyAdditiveLayer _upperBodyAdditiveLayer;
		[Inject] private FatalityLayer _fatalityLayer;
		[Inject] private AttackLayer _attackLayer;
		[Inject] private IPausableInfo _pausable;
		[Inject] private BaseLayer _baseLayer;
		
		public IReadOnlyUpperBodyAdditiveLayer UpperBodyAdditiveLayer => _upperBodyAdditiveLayer;
		public IReadOnlyAnimatorLayer FatalityLayer => _fatalityLayer;
		public IReadOnlyAnimatorLayer AttackLayer => _attackLayer;
		public IReadOnlyAnimatorLayer BaseLayerInfo => _baseLayer;

		protected override void Awake()
		{
			base.Awake();
			
			if (!_isUseUpperBodyAdditiveLayer)
				_upperBodyAdditiveLayer.Disable();
		}
		
		public void PlayFatalityAnim(FatalityType type)
		{
			switch (type)
			{
				case FatalityType.NeckSlash:
				default:
					_fatalityLayer.PlayNeckSlashAnim();
					break;
				case FatalityType.StabStomach:
					_fatalityLayer.PlayStabStomachAnim();
					break;
			}
		}
		
		public void ResetIdleUpperBodyAdditiveAnim() => _upperBodyAdditiveLayer.ResetIdleAnim();
		
		public void ResetWalkUpperBodyAdditiveAnim() => _upperBodyAdditiveLayer.ResetWalkAnim();
		
		public void ResetRunUpperBodyAdditiveAnim() => _upperBodyAdditiveLayer.ResetRunAnim();
		
		public void PlayIdleUpperBodyAdditiveAnim() => _upperBodyAdditiveLayer.PlayIdleAnim();
		
		public void PlayWalkUpperBodyAdditiveAnim() => _upperBodyAdditiveLayer.PlayWalkAnim();
		
		public void PlayRunUpperBodyAdditiveAnim() => _upperBodyAdditiveLayer.PlayRunAnim();
		
		public void PlayAttackAnim() => _attackLayer.PlayAttackAnim();
		
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
			
			AutoLayerWeightControl(_fatalityLayer);
			AutoLayerWeightControl(_attackLayer);
			
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