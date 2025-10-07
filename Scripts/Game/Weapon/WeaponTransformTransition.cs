using UnityEngine;

namespace FAS
{
	public sealed class WeaponTransformTransition
	{
		private Transform _transform;
		
		private Quaternion _startRotation;
		
		private Vector3 _startScale;
		private Vector3 _startPos;

		private float _inverseDuration;
		private float _startTime;

		private bool _isActive;
		
		public bool IsActive => _isActive;

		public void StartTransition(Transform target, Transform newParent, float duration)
		{
			if (_isActive)
				SnapToTarget();

			_transform = target;
			_transform.SetParent(newParent);

			if (duration > 0f)
			{
				_startPos = _transform.localPosition;
				_startRotation = _transform.localRotation;
				_startScale = _transform.localScale;

				_startTime = Time.timeSinceLevelLoad;
				_inverseDuration = 1f / duration;

				_isActive = true;
			}
			else
			{
				SnapToTarget();
				_isActive = false;
			}
		}

		private void SnapToTarget()
		{
			_transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			_transform.localScale = Vector3.one;
		}

		public void CancelAndSnap()
		{
			if (_isActive)
			{
				SnapToTarget();
				_isActive = false;	
			}
		}
		
		public void Update()
		{
			var now = Time.timeSinceLevelLoad;
			var t = (now - _startTime) * _inverseDuration;

			if (t > 1f)
			{
				SnapToTarget();
				_isActive = false;
			}
			else
			{
				_transform.localPosition = Vector3.Lerp(_startPos, Vector3.zero, t);
				_transform.localRotation = Quaternion.Slerp(_startRotation, Quaternion.identity, t);
				_transform.localScale = Vector3.Lerp(_startScale, Vector3.one, t);	
			}
		}
	}
}