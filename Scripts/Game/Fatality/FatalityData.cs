using UnityEngine;
using System;

namespace FAS.Fatality
{
	[Serializable]
	public struct FatalityData
	{
		public FatalityType Type;
		[SerializeField] private Transform _targetPoint;
		[SerializeField] private Transform _cameraPoint;
		
		public Transform CameraPoint => _cameraPoint;
		
		public Vector3 RotationAngles => _targetPoint.eulerAngles;
		public Quaternion Rotation => _targetPoint.rotation;
		public Vector3 Position => _targetPoint.position;
	}
}