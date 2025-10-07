using UnityEngine;

namespace FAS
{
	public interface IReadOnlyPatrolPoint
	{
		public Vector3 Position { get; }
		
		public float HorizontalRotation { get; }
		public float IdleTime { get; }

		public bool IsReached { get; }

		public float GetIdleAnimValue();
	}
}