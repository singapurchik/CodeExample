using UnityEngine;
using FAS.Players;
using FAS.Sounds;
using Zenject;
using System;

namespace FAS.Actors
{
	public class TargetDetector : MonoBehaviour
	{
		[SerializeField] private SoundEvent _detectionCue;
		[SerializeField] private float _distanceToLoseTarget = 5f;
		
		[Inject] private ISoundEffectsPlayer _soundEffects;
		[Inject] private VisionSensor _visionSensor;
		[Inject] private TargetDetectorView _view;
		[Inject] private IPausableInfo _pausable;

		private bool _isDisableRequested;
		private bool _isActive = true;
		
		public ITargetPlayer CurrentTarget { get; private set; }

		public Vector3 TargetPosition => CurrentTarget.Position;
		
		public bool IsTargetDetected => CurrentTarget != null;
		
		public event Action OnTargetDetected;
		public event Action OnTargetMissing;
		
		private void OnEnable()
		{
			_visionSensor.OnTargetDetected += InvokeOnTargetDetected;
		}

		private void OnDisable()
		{
			_visionSensor.OnTargetDetected -= InvokeOnTargetDetected;
			CurrentTarget = null;
			_view.Hide();
		}
		
		public void RequestDisable()
		{
			_isDisableRequested = true;
		}

		private void InvokeOnTargetDetected(ITargetPlayer target)
		{
			_soundEffects.PlayOneShot(_detectionCue);
			_view.Show();
			_view.UpdateView(1);
			CurrentTarget = target;
			OnTargetDetected?.Invoke();
		}

		private void LoseTarget()
		{
			_visionSensor.ResetDetectionNextFrame();
			OnTargetMissing?.Invoke();
			CurrentTarget = null;
			_view.Hide();
		}

		private void Disable()
		{
			LoseTarget();
			_isActive = false;
		}

		private void Update()
		{
			if (_pausable.IsPaused) return;
			
			if (_isDisableRequested)
			{
				if (_isActive)
					Disable();

				_isDisableRequested = false;
			}
			else
			{
				if (IsTargetDetected)
				{
					var targetPosition = CurrentTarget.Position;
					targetPosition.y = transform.position.y;
					
					if (Vector3.SqrMagnitude(transform.position - targetPosition)
					    > _distanceToLoseTarget * _distanceToLoseTarget)
							LoseTarget();
				}
				else
				{
					_isActive = true;

					_visionSensor.RequestUpdate();

					if (_visionSensor.DetectionProgressNormalized <= 0)
					{
						if (_view.IsShown && !_view.IsProcessHide)
							_view.Hide();
					}
					else if (!_view.IsShown && !_view.IsProcessShow)
					{
						_view.Show();
					}

					_view.UpdateView(_visionSensor.DetectionProgressNormalized);
				}
			}
		}
	}
}