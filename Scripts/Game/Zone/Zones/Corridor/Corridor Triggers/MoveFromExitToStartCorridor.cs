using UnityEngine;

namespace FAS.Triggers.Corridor
{
	public class MoveFromExitToStartCorridor : TriggerVisitable
	{
		[SerializeField] private Transform _finishPoint;
		[SerializeField] private Transform _startPoint;
		
		public Transform FinishPoint => _finishPoint;
		public Transform StartPoint => _startPoint;
		
		public override void Accept(ITriggerVisitor visitor) => visitor.Apply(this);
	}
}