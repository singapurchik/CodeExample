using Random = UnityEngine.Random;
using UnityEngine;
using System;

namespace FAS.Actors
{
	[Serializable]
	public class AnimatorStateIntervalAction
	{
		[SerializeField] private float _minInterval = 1f;
		[SerializeField] private float _maxInterval = 3f;

		private IReadOnlyAnimatorLayer _animInfo;

		private Action _resetAction;
		private Action _action;
		
		private float _nextInvokeTime;
		
		private int _animHash;

		public void Initialize(IReadOnlyAnimatorLayer animInfo, int animHash, Action action, Action resetAction)
		{
			_animInfo = animInfo;
			_animHash = animHash;
			_action = action;
			_resetAction = resetAction;
		}

		public void Reset() => _resetAction?.Invoke();

		public void UpdateInterval()
		{
			_nextInvokeTime =
				Time.timeSinceLevelLoad + Random.Range(_minInterval, _maxInterval);
		}
		
		public void TryInvoke()
		{
			if (_animInfo.CurrentAnimHash != _animHash)
			{
				if (Time.timeSinceLevelLoad > _nextInvokeTime)
					_action?.Invoke();
			}
			else
			{
				UpdateInterval();
			}
		}
	}
}