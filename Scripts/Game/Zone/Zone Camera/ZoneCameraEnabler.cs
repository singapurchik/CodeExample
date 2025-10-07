using UnityEngine.Events;
using Cinemachine;
using FAS.Players;
using UnityEngine;

namespace FAS
{
	public class ZoneCameraEnabler : MonoBehaviour
	{
		[SerializeField] private CinemachineVirtualCamera _camera;
		[SerializeField] private float _enableDelay;
		[SerializeField] private float _disableDelay;

		private float _nextDisableTime;
		private float _nextEnableTime;

		private bool _isDisableRequested;
		private bool _isEnableRequested;
		private bool _isPlayerInside;
		private bool _isCameraActive;

		public UnityEvent OnDisable;
		public UnityEvent OnEnable;


		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Player player))
				RequestEnable();
		}

		private void OnTriggerStay(Collider other)
		{
			if (other.TryGetComponent(out Player player))
				_isPlayerInside = true;
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out Player player))
				RequestDisable();
		}

		private void RequestDisable()
		{
			_isDisableRequested = true;
			_isEnableRequested = false;
			_nextDisableTime = Time.timeSinceLevelLoad + _disableDelay;
		}

		private void RequestEnable()
		{
			_isDisableRequested = false;
			_isEnableRequested = true;
			_nextEnableTime = Time.timeSinceLevelLoad + _enableDelay;
		}

		private void Enable()
		{
			if (!_isCameraActive)
			{
				_camera.gameObject.SetActive(true);
				_isCameraActive = true;
				OnEnable?.Invoke();
			}

			_isEnableRequested = false;
		}

		private void Disable()
		{
			if (_isCameraActive)
			{
				_camera.gameObject.SetActive(false);
				_isCameraActive = false;
				OnDisable?.Invoke();
			}

			_isDisableRequested = false;
		}

		private const float AUTO_DISABLE_DELAY = 0.25f;
		private float _timeToAutoDisable;
		
		private void Update()
		{
			if (_isCameraActive)
			{
				if (_isPlayerInside)
					_timeToAutoDisable = Time.timeSinceLevelLoad + AUTO_DISABLE_DELAY;
				else if (Time.timeSinceLevelLoad > _timeToAutoDisable)
					_isDisableRequested = true;
			}
			
			if (_isEnableRequested && Time.timeSinceLevelLoad > _nextEnableTime)
				Enable();

			if (_isDisableRequested && Time.timeSinceLevelLoad > _nextDisableTime)
				Disable();
		}

		private void FixedUpdate()
		{
			_isPlayerInside = false;
		}
	}
}