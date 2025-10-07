using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace FAS
{
	public class ActiveCameraData : MonoBehaviour
	{
		[Inject] private List<ActiveCameraListener> _cameraListeners;
		[Inject] private ICameraBlendsChanger _cameraBlendsChanger;
		
		public static OverrideFollowCameraSide OverrideFollowCameraSide { get; private set; }
		public static CameraMoveType MoveType { get; private set; }
		public static CameraType Type { get; private set; }

		private void OnEnable()
		{
			foreach (var cameraListener in _cameraListeners)
				cameraListener.OnActivated.AddListener(OnCameraChanged);
		}
		
		private void OnDisable()
		{
			foreach (var cameraListener in _cameraListeners)
				cameraListener.OnActivated.RemoveListener(OnCameraChanged);
		}

		private void OnCameraChanged(CameraData data)
		{
			OverrideFollowCameraSide = data.OverrideFollowCameraSide;
			MoveType = data.CameraMoveType;
			Type = data.CameraType;
		}
	}
}