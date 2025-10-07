using UnityEngine;
using Zenject;

namespace FAS.UI
{
	public class BillboardToCamera : MonoBehaviour
	{
		[SerializeField] private float _updateInterval = 0.1f;
		[SerializeField] private bool _ignoreYAxis = true;

		[Inject(Optional = true)] private Camera _mainCamera;

		private Transform _cameraTransform;
		
		private float _nextUpdateTime;

		private void Awake()
		{
			if (_mainCamera == null)
				_mainCamera = Camera.main;
			
			_cameraTransform = _mainCamera.transform;
		}

		private void LateUpdate()
		{
			if (Time.timeSinceLevelLoad > _nextUpdateTime)
			{
				var direction = transform.position - _cameraTransform.position;

				if (_ignoreYAxis)
					direction.y = 0f;

				if (direction.sqrMagnitude > 0.001f)
					transform.forward = direction.normalized;	
				
				_nextUpdateTime = Time.time + _updateInterval;
			}
		}

		public void ForceUpdateNow()
		{
			_nextUpdateTime = 0f;
		}
	}
}