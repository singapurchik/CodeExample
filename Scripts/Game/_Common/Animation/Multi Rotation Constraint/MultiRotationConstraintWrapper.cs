using UnityEngine.Animations.Rigging;
using UnityEngine;

namespace FAS
{
	public class MultiRotationConstraintWrapper : WeightChanger
	{
		private Quaternion _targetLocalRotation;
		private MultiRotationConstraint _rig;
		private Transform _target;
		
		private float _rotationSpeed;
		private float _baseOffset;
		
		public void SetData(MultiRotationConstraintData data)
		{
			_rig = data.Rig;
			_target = data.Target;
			_rotationSpeed = data.RotationSpeed;
			_baseOffset = data.BaseOffset;
			base.SetData(data.DefaultChangeWeightSpeed, data.MaxWeight);
		}

		public void RequestEnable(float yawDeg)
		{
			_targetLocalRotation = Quaternion.Euler(0, _baseOffset + yawDeg, 0f);
			base.RequestEnable();
		}

		public void ForceUpdateWeight()
		{
			_rig.weight = Weight;
		}

		public void Update()
		{
			RotateTarget();
			
			if (IsWeightChanged(out var weight))
				_rig.weight = weight;
		}

		private void RotateTarget()
		{
			var current = _target.localRotation;

			if (_rig.weight <= 0)
			{
				_target.localRotation = _targetLocalRotation;
			}
			else if (!IsClose(current, _targetLocalRotation))
			{
				var next = Quaternion.RotateTowards(current, _targetLocalRotation,  
					_rotationSpeed * Time.deltaTime);
				
				if (!IsClose(current, next, 1e-6f))
					_target.localRotation = next;
			}
		}

		private bool IsClose(Quaternion a, Quaternion b, float eps = 1e-4f)
			=> 1f - Mathf.Abs(Quaternion.Dot(a, b)) <= eps;
	}
}