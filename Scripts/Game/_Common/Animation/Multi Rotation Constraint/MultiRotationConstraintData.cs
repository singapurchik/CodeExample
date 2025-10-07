using UnityEngine.Animations.Rigging;
using UnityEngine;
using System;

namespace FAS
{
	[Serializable]
	public class MultiRotationConstraintData
	{
		[SerializeField] private MultiRotationConstraint _rig;
		[SerializeField] private Transform _target;
		[Range(0, 1)] [SerializeField] private float _maxWeight = 1f;
		[SerializeField] private float _defaultChangeWeightSpeed = 5f;
		[SerializeField, Tooltip("Degrees per second")] private float _rotationSpeed = 180f;
		[SerializeField, Tooltip("Base local pitch offset, degrees")] private float _baseOffset;

		public MultiRotationConstraint Rig => _rig;
		public Transform Target => _target;
		
		public float DefaultChangeWeightSpeed => _defaultChangeWeightSpeed;
		public float RotationSpeed => _rotationSpeed;
		public float BaseOffset => _baseOffset;
		public float MaxWeight => _maxWeight;
	}
}