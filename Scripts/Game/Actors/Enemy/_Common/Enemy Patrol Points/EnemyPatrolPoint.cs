using System.Collections.Generic;
using UnityEngine;

namespace FAS.Actors.Emenies
{
	public class EnemyPatrolPoint : PatrolPoint
	{
		[SerializeField] private List<IdleAnimName> _idleAnimations;

		private List<IdleAnimName> _shuffledAnimations;
		private int _currentAnimationIndex;

		private void Awake()
		{
			_shuffledAnimations = new List<IdleAnimName>(_idleAnimations);
			ShuffleAnimations();
		}

		public override float GetIdleAnimValue()
		{
			if (_idleAnimations.Count == 1)
				return (float)_idleAnimations[0];

			if (_currentAnimationIndex >= _shuffledAnimations.Count)
			{
				ShuffleAnimations();
				_currentAnimationIndex = 0;
			}

			return (float)_shuffledAnimations[_currentAnimationIndex++];
		}

		private void ShuffleAnimations()
		{
			for (var i = _shuffledAnimations.Count - 1; i > 0; i--)
			{
				var j = Random.Range(0, i + 1);
				(_shuffledAnimations[i], _shuffledAnimations[j]) = (_shuffledAnimations[j], _shuffledAnimations[i]);
			}
		}
	}
}