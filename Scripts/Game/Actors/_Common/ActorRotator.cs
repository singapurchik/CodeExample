using UnityEngine.AI;
using UnityEngine;
using Zenject;

namespace FAS.Actors
{
	public class ActorRotator : MonoBehaviour
	{
		[Inject] private IPausableInfo _pausable;
		[Inject] private NavMeshAgent _agent;

		private Quaternion _requestedRotation;
		
		private float _requestedRotationAngle;
		private float _requestedRotationSpeed;
		
		private bool _isForceRotationRequested;
		private bool _isRotationRequested;
		
		public void SetAutoAngularSpeed(float speed) => _agent.angularSpeed = speed;

		public void RequestForceRotateHorizontal(Quaternion rotation)
		{
			_requestedRotation = rotation;
			_isForceRotationRequested = true;
		}

		public void RequestRotateHorizontal(float angle, float speed = 360)
		{
			_requestedRotationAngle = angle;
			_requestedRotationSpeed = speed;
			_isRotationRequested = true;
		}

		public bool IsHorizontalAngelSames(float targetAngle)
			=> Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) < 0.1f;

		private void Update()
		{
			if (_pausable.IsPaused) return;

			if (_isForceRotationRequested)
			{
				transform.rotation = _requestedRotation;
				_isForceRotationRequested = false;
				_isRotationRequested = false;
			}
			else if (_isRotationRequested)
			{
				transform.rotation = Quaternion.Slerp(
					transform.rotation, Quaternion.Euler(0f, _requestedRotationAngle, 0f),
					_requestedRotationSpeed * Time.deltaTime);

				_isRotationRequested = false;
			}
		}
	}
}