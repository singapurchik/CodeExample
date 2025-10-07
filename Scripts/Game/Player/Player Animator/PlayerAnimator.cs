using System.Collections.Generic;
using FAS.Fatality;
using UnityEngine;
using Zenject;

namespace FAS.Players.Animations
{
	public class PlayerAnimator : CharacterAnimator
	{
		[SerializeField] private float _changeHealthBehaviourTypeValueSpeed = 2f;
		[SerializeField] private float _increaseLocomotionSpeed = 2f;
		[SerializeField] private float _decreaseLocomotionSpeed = 2f;
		
		[InjectOptional] private List<ILayerWithTriggers> _layersWithTriggers;
		[Inject] private UpperBodyAdditiveLayer _upperBodyAdditiveLayer;
		[Inject] private FullBodyReactionLayer _fullBodyReactionLayer;
		[Inject] private RightHandLayer _rightHandLayer;
		[Inject] private WeaponLayer _weaponLayer;
		[Inject] private FatalityLayer _fatalityLayer;
		[Inject] private IPausableInfo _pausable;
		[Inject] private BaseLayer _baseLayer;
		
		public Vector3 DeltaPosition => Animator.deltaPosition;
		
		private float _currentHealthBehaviourTypeValue;
		private float _locomotionValueRequested;
		private float _nextTimeUpdateArmLayer;
		
		private bool _isChangeLocomotionRequested;
		
		public IReadOnlyFullBodyReactionLayer FullBodyReactionLayer => _fullBodyReactionLayer;
		public IReadOnlyRightHandLayer RightHandLayer => _rightHandLayer;
		public IReadOnlyAnimatorLayer FatalityLayer => _fatalityLayer;
		public IReadOnlyWeaponLayer WeaponLayer => _weaponLayer;
		public IReadOnlyBaseLayer BaseLayer => _baseLayer;

		public void SmoothChangeHealthBehaviorType(float value) => _currentHealthBehaviourTypeValue = value;
		
		public void PlayShootAnim(bool isUseBlending) => _weaponLayer.PlayShootingAnim(isUseBlending);
		
		public void SetIsHasAmmoState(bool value) => _weaponLayer.SetIsHasAmmoState(value);

		public void PlaySniperRifleAiming() => _weaponLayer.PlayReadySniperRifle();
		
		public void PlayFinishAiming() => _weaponLayer.PlayFinishAiming();
		
		public void StopShootingAnim() => _weaponLayer.StopShootingAnim();
		
		public void PlayStartAiming() => _weaponLayer.PlayStartAiming();
		
		public void PlayInteractionAnim(RightHandAnimType animType)
		{
			switch (animType)
			{
				case RightHandAnimType.Interact:
				default:
					_rightHandLayer.PlayInteractAnim();
					break;
			}
		}
		
		public void PlayFromSlipToFlyingBackAnim() => _fullBodyReactionLayer.PlayFromSlipToFlyingBackAnim();
		
		public void PlayKnockedDownAnim() => _fullBodyReactionLayer.PlayKnockedDownAnim();
		
		public void PlayPickUpAnim() => _rightHandLayer.PlayPickUpAnim();
		
		public float GetLocomotionValue() => _baseLayer.LocomotionValue;
		
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

		public void RequestSetLocomotionValue(float value)
		{
			_isChangeLocomotionRequested = true;
			_locomotionValueRequested = value;
		}
		
		public void ForceSetLocomotionValue(float value) => _baseLayer.SetLocomotionValue(value);
		
		public void PlayTakeDamageAnim() => _upperBodyAdditiveLayer.PlayTakeDamageAnim();
		
		public void EnableStandLocomotion() => _baseLayer.SetIsCrouchedState(false);

		public void EnableCrouchLocomotion() => _baseLayer.SetIsCrouchedState(true);

		public bool IsCrouched() => _baseLayer.GetIsCrouchedState();
		
		private void SmoothChangeLocomotionValue(float value, float speed)
		{
			var targetValue = Mathf.MoveTowards(_baseLayer.LocomotionValue,
				value, speed * Time.deltaTime);
			_baseLayer.SetLocomotionValue(targetValue);
		}
		
		public void ForceChangeLocomotionValue(float value)
		{
			_locomotionValueRequested = value;
			_baseLayer.SetLocomotionValue(_locomotionValueRequested);
		}

		private void UpdateLocomotionValue()
		{
			if (_isChangeLocomotionRequested)
			{
				if (IsForceUpdateThisFrame || 
				    !Mathf.Approximately(_baseLayer.LocomotionValue, _locomotionValueRequested))
				{
					if (_baseLayer.LocomotionValue < _locomotionValueRequested)
						SmoothChangeLocomotionValue(_locomotionValueRequested, _increaseLocomotionSpeed);
					else
						SmoothChangeLocomotionValue(_locomotionValueRequested, _decreaseLocomotionSpeed);
				}

				_isChangeLocomotionRequested = false;
			}
			else if (_baseLayer.LocomotionValue > 0)
			{
				SmoothChangeLocomotionValue(_locomotionValueRequested, _decreaseLocomotionSpeed);
			}
		}
		
		private void UpdateHealthBehaviourTypeValue()
		{
			if (IsForceUpdateThisFrame
			    || !Mathf.Approximately(_baseLayer.HealthBehaviourTypeValue, _currentHealthBehaviourTypeValue))
			{
				var targetValue = Mathf.MoveTowards(_baseLayer.HealthBehaviourTypeValue,
					_currentHealthBehaviourTypeValue, _changeHealthBehaviourTypeValueSpeed * Time.deltaTime);
				_baseLayer.SetHealthBehaviourTypeValue(targetValue);
			}
		}

		private void AutoLayerWeightControl(Layer layer)
		{
			if (layer.IsActive && !(layer.IsInTransition && layer.NextAnimHash == Layer.EmptyAnimHash))
			{
				if (!layer.IsEnabled)
					layer.EnableWeightSmooth();
			}
			else if (!layer.IsDisabled)
			{
				layer.DisableWeightSmooth();
			}
		}

		private void Update()
		{
			if (_pausable.IsPaused) return;
			
			AutoLayerWeightControl(_fullBodyReactionLayer);
			AutoLayerWeightControl(_weaponLayer);
			AutoLayerWeightControl(_fatalityLayer);
			UpdateHealthBehaviourTypeValue();
			UpdateLocomotionValue();
			IsForceUpdateThisFrame = false;
		}
	}
}