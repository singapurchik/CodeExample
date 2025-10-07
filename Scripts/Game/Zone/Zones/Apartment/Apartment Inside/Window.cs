using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using FAS.Utils;

namespace FAS.Apartments.Inside
{
	[Serializable]
	public class Window
	{
		[SerializeField] private CinemachineVirtualCamera _camera;
		[SerializeField] private List<GameObject> _outsideParts;
		[SerializeField] private TextureSelector _textureSelector;

		private const float DEFAULT_CAMERA_ANGLE = -90f;
		private const float MIN_CAMERA_ANGLE = -123.5f;
		private const float MAX_CAMERA_ANGLE = -36.5f;

		public void SetTexture(Texture texture) => _textureSelector.SetAndApplyTexture(texture);

		public void HideOutsideParts()
		{
			foreach (var outsidePart in _outsideParts)
				outsidePart.TryDisable();
		}
		
		public void ShowOutsideParts()
		{
			foreach (var outsidePart in _outsideParts)
				outsidePart.TryEnable();
		}

		public void RotateCamera(float delta)
		{
			float currentRotationY = _camera.transform.localEulerAngles.y;
			float targetRotationY = Mathf.Repeat(currentRotationY + delta, 360f);
			
			if (targetRotationY > 180f)
				targetRotationY -= 360f;

			targetRotationY = Mathf.Clamp(targetRotationY, MIN_CAMERA_ANGLE, MAX_CAMERA_ANGLE);
			_camera.transform.localRotation = Quaternion.Euler(0, targetRotationY, 0);
		}

		public void ResetCameraRotation()
		{
			var targetRotation = _camera.transform.localEulerAngles;
			targetRotation.y = DEFAULT_CAMERA_ANGLE;
			_camera.transform.localRotation = Quaternion.Euler(targetRotation);
		}
	}
}