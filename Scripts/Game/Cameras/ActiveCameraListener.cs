using UnityEngine.Events;
using UnityEngine;
using Cinemachine;
using VInspector;
using Zenject;

namespace FAS
{
	public class ActiveCameraListener : MonoBehaviour
	{
		[SerializeField] private CinemachineVirtualCameraBase _virtualCamera;
		[SerializeField] private CameraData _data;
		
		[Inject] private ICinemachineBrainInfo _brainInfo;
		
		private bool _wasActive;
		
		public UnityEvent<CameraData> OnDeactivated;
		public UnityEvent<CameraData> OnActivated;

		private void Update()
		{
			var isNowActive = _brainInfo.ActiveCamera == _virtualCamera;

			if (isNowActive)
			{
				if (!_wasActive)
					OnActivated?.Invoke(_data);
			}
			else if (_wasActive)
			{
				OnDeactivated?.Invoke(_data);
			}

			_wasActive = isNowActive;
		}

#if UNITY_EDITOR
		[Button]
		private void FindCamera()
		{
			_virtualCamera = GetComponentInChildren<CinemachineVirtualCameraBase>(true);
		}
#endif
	}
}
