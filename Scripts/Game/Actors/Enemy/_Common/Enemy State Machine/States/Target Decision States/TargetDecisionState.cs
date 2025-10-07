namespace FAS.Actors.Emenies
{
	public abstract class TargetDecisionState : EnemyState
	{
		private const float MIN_DISTANCE_TO_TARGET = 1f;
		
		public override void Perform()
		{
			if (!IsTransitionBlocked())
			{
				if (TargetDetector.IsTargetDetected)
					OnTargetDetected();
				else
					OnNoTargetDetected();	
			}
		}

		protected virtual void OnTargetDetected()
		{
			if (IsTargetNear(MIN_DISTANCE_TO_TARGET))
				RequestTransitionToAggressiveState();
			else if (Key != EnemyStates.Chase)
				RequestTransition(EnemyStates.Chase);
		}
		
		protected virtual void OnNoTargetDetected()
		{
			
		}

		protected void RequestTransitionToAggressiveState()
		{
			if (TargetDetector.CurrentTarget.Health.Value > 0)
			{
				RequestTransition(EnemyStates.Attack);
			}
			else
			{
				if (FatalityTarget.IsEmpty)
					RequestTransition(EnemyStates.FatalityExecute);
				else
					transform.parent.gameObject.SetActive(false);
			}
		}
	}
}