using UnityEngine;
using System;

namespace FAS
{
	[Serializable]
	public class WeightChanger
	{
		protected float Weight { get; private set; }
		
		private float _defaultChangeWeightSpeed;
		private float _maxWeight;
		
		private float _requestedWeight;

		private bool _isEnableRequested;

		public void SetData(float defaultChangeWeightSpeed, float maxWeight)
		{
			_defaultChangeWeightSpeed = defaultChangeWeightSpeed;
			_maxWeight = maxWeight;
		}
		
		public void RequestEnable()
		{
			_requestedWeight = Mathf.Clamp01(_maxWeight);
			_isEnableRequested = true;
		}

		public bool IsWeightChanged(out float newWeight, float speed = 0)
		{
			float target = _isEnableRequested ? _requestedWeight : 0f;

			if (Mathf.Approximately(Weight, target))
			{
				newWeight = Weight;
				_isEnableRequested = false;
				return false;
			}

			if (speed <= 0f)
				speed = _defaultChangeWeightSpeed;
			
			Weight = Mathf.MoveTowards(Weight, target, speed * Time.deltaTime);
			newWeight = Weight;
			_isEnableRequested = false;
			return true;
		}
	}
}