using UnityEngine;

namespace FAS
{
	public abstract class PatrolPoint : MonoBehaviour, IReadOnlyPatrolPoint
	{
		[SerializeField] private float _minIdleTime = 3f;
		[SerializeField] private float _maxIdleTime = 10f;
		
		public Vector3 Position => transform.position;
		
		public float IdleTime => Random.Range(_minIdleTime, _maxIdleTime);
		public float HorizontalRotation => transform.eulerAngles.y;

		public bool IsReached { get; private set; }
		
		public abstract float GetIdleAnimValue();
		
		public void SetReached(bool value) => IsReached = value;
	}
}