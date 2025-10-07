using System.Collections.Generic;
using System.Collections;
using FAS.Fatality;
using UnityEngine;
using System.Linq;

namespace FAS.Actors.Emenies
{
	public class FatalityExecute : EnemyState
	{
		[SerializeField] private List<FatalityData> _fatalityData;
		[SerializeField] private FatalityType _fatalityType;

		private FatalityData _currentData;
		
		public override EnemyStates Key => EnemyStates.FatalityExecute;
		
		private void Awake()
		{
			_currentData = _fatalityData.First(data => data.Type == _fatalityType);
		}

		public override void Enter()
		{
			Mover.TryStopMove();
			FatalityTarget.PrepareFatality(_currentData);
			StartCoroutine(PlayAnimation());
		}

		private IEnumerator PlayAnimation()
		{
			yield return null;
			Animator.PlayFatalityAnim(_currentData.Type);
			FatalityTarget.PerformFatality();
		}

		public override void Perform()
		{
			TargetDetector.RequestDisable();
			
			if (FatalityTarget.IsFatalityCompleted)
				RequestTransition(EnemyStates.ShowingFace);
		}
	}
}