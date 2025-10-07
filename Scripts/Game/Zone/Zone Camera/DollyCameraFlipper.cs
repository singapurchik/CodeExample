using Cinemachine;
using UnityEngine;

namespace FAS
{
	public class DollyCameraFlipper : MonoBehaviour
	{
		[SerializeField] private float _checkDuration = 0.5f;
		[SerializeField] private float _changeOffsetSpeed = 8f;

		private CinemachineTrackedDolly _trackedDolly;
		
		private float _originalOffset;
		private float _lastPathPosition;
		private float _directionTimer;

		private int _currentDirection;
		
		private bool _isInverted;

		private void Awake()
		{
			_trackedDolly = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();
			_originalOffset = _trackedDolly.m_AutoDolly.m_PositionOffset;
			_lastPathPosition = _trackedDolly.m_PathPosition;
			enabled = false;
		}

		private void UpdateDirection()
		{
			var currentPosition = _trackedDolly.m_PathPosition;
			var delta = currentPosition - _lastPathPosition;
			_lastPathPosition = currentPosition;

			int direction = Mathf.Abs(delta) < 0.001f ? 0 : (delta < 0 ? -1 : 1);

			if (direction == _currentDirection)
			{
				if (direction != 0)
					_directionTimer += Time.deltaTime;
			}
			else
			{
				_currentDirection = direction;
				_directionTimer = 0f;
			}
		}

		private void HandleInversion()
		{
			var shouldInvert = _currentDirection == -1 && !_isInverted && _directionTimer > _checkDuration;
			var shouldRestore = _currentDirection == 1 && _isInverted && _directionTimer > _checkDuration;

			if (shouldInvert)
			{
				_isInverted = true;
				_directionTimer = 0f;
			}

			if (shouldRestore)
			{
				_isInverted = false;
				_directionTimer = 0f;
			}
		}

		private void InterpolateOffset()
		{
			var targetOffset = _isInverted ? -_originalOffset : _originalOffset;
			var currentOffset = _trackedDolly.m_AutoDolly.m_PositionOffset;

			_trackedDolly.m_AutoDolly.m_PositionOffset = Mathf.MoveTowards(
				currentOffset, targetOffset, _changeOffsetSpeed * Time.deltaTime);
		}
		
		private void Update()
		{
			UpdateDirection();
			HandleInversion();
			InterpolateOffset();
		}
	}
}
