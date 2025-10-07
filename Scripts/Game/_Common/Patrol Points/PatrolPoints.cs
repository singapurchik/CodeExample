using System.Collections.Generic;
using UnityEngine;

namespace FAS
{
	public abstract class PatrolPoints<T> : MonoBehaviour, IPatrolPointHolder<T> where T : PatrolPoint
	{
		[SerializeField] protected List<T> Points = new();

		public IReadOnlyPatrolPoint Current => _current;
		public IReadOnlyPatrolPoint Next  => _next;

		private T _current;
		private T _next;
		
		private int _currentPointIndex;

		private void Awake()
		{
			_current = Points[_currentPointIndex];
			_current.SetReached(true);
			UpdateNextPoint();
    
			transform.SetParent(null);
		}

		public void SetPointReached()
		{
			_current.SetReached(false);
			_currentPointIndex++;

			if (_currentPointIndex >= Points.Count)
				_currentPointIndex = 0;

			_current = Points[_currentPointIndex];
			_current.SetReached(true);
			UpdateNextPoint();
		}

		private void UpdateNextPoint()
		{
			if (_currentPointIndex + 1 < Points.Count)
				_next = Points[_currentPointIndex + 1];
			else
				_next = Points[0];
		}

#if UNITY_EDITOR
		[SerializeField] private bool _drawGizmos = true;
		[SerializeField] private float _pointRadius = 0.2f;
		[SerializeField] private float _directionLength = 1f;

		private void OnDrawGizmos()
		{
			if (!_drawGizmos || Points == null || Points.Count == 0)
				return;

			for (int i = 0; i < Points.Count; i++)
			{
				if (Points[i] == null)
					continue;

				Gizmos.color = Points[i] == Current ? Color.green : Color.magenta;
				Gizmos.DrawSphere(Points[i].Position, _pointRadius);

				Gizmos.color = Color.blue;
				Vector3 forwardDirection = Quaternion.Euler(0f, Points[i].HorizontalRotation, 0f) * Vector3.forward;
				Gizmos.DrawLine(Points[i].Position + (Vector3.up * 0.5f),
					Points[i].Position + (Vector3.up * 0.5f) + forwardDirection * _directionLength);


				if (i < Points.Count - 1)
				{
					Gizmos.color = Color.white;
					Gizmos.DrawLine(Points[i].Position, Points[i + 1].Position);
				}
				else
				{
					Gizmos.color = Color.white;
					Gizmos.DrawLine(Points[i].Position, Points[0].Position);
				}
			}
		}
#endif
	}
}