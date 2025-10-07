using UnityEngine;
using Zenject;

namespace FAS
{
	public class LookAtCamera : MonoBehaviour
	{
		[SerializeField] private float _updateInterval = 0.25f;
		
		[Inject(Optional = true)] private Camera _camera;
		
		private Transform _transform;
		
		private float _nextUpdateTime;
		
		private void Awake()
		{
			if (_camera == null)
				_camera = Camera.main;

			_transform = transform;
		}

		public void OnUpdate()
		{
			_transform.LookAt(_transform.position + _camera.transform.forward, _camera.transform.up);
			_nextUpdateTime = Time.timeSinceLevelLoad + _updateInterval;
		}

		private void Update()
		{
			if (Time.timeSinceLevelLoad > _nextUpdateTime)
				OnUpdate();
		}
	}
}