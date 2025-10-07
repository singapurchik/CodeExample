using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerRotator : MonoBehaviour
	{
		[Inject (Id = CharacterTransformType.Body)] private Transform _body;
		[Inject] private Camera _mainCamera;
		
		public void LookAt(Vector3 targetPosition)
		{
			var directionToTarget = targetPosition - transform.position;
			var targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
			transform.rotation = targetRotation;

			var fixedAngles = transform.eulerAngles;
			fixedAngles.x = 0;
			fixedAngles.z = 0;
			transform.eulerAngles = fixedAngles;
		}

		public void ForceRotateHorizontal(float angle) => transform.rotation = Quaternion.Euler(0f, angle, 0f);
		
		public void ForceRotateToTarget(Quaternion target) => transform.rotation = target;
		
		public void ForceRotateToCameraView()
		{
			var cameraForward = _mainCamera.transform.forward;
			cameraForward.y = 0f;
			cameraForward.Normalize();

			var cameraYAngle = Mathf.Atan2(cameraForward.x, cameraForward.z) * Mathf.Rad2Deg;
			ForceRotateHorizontal(cameraYAngle);
		}

		public void SmoothRotateToDirection(Vector3 direction, float speed)
		{
			if (direction.sqrMagnitude > 0.0001f)
			{
				var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
			}
		}
		
		public void SmoothRotateHorizontal(Vector3 direction, float speed)
		{
			if (direction.sqrMagnitude > 0.0001f)
			{
				direction.y = 0f;
				
				var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
				SmoothRotateHorizontal(targetRotation.eulerAngles.y, speed);
			}
		}

		public void SmoothRotateHorizontal(float targetY, float speed)
		{
			var currentY = transform.eulerAngles.y;
			var newY = Mathf.MoveTowardsAngle(currentY, targetY, speed * Time.deltaTime);
			transform.rotation = Quaternion.Euler(0f, newY, 0f);
		}

		public bool TryRotateToTargetHorizontal(Vector3 targetPosition, float speed)
		{
			targetPosition.y = transform.position.y;
			var direction = targetPosition - transform.position;

			bool isRotating = false;

			if (direction.sqrMagnitude > 0.000001f)
			{
				var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
				var targetY = targetRotation.eulerAngles.y;
				var currentY = transform.eulerAngles.y;

				var angleDelta = Mathf.DeltaAngle(currentY, targetY);
				var step = speed * Time.deltaTime;

				if (Mathf.Abs(angleDelta) > 0.1f)
				{
					var newY = Mathf.MoveTowardsAngle(currentY, targetY, step);
					transform.rotation = Quaternion.Euler(0f, newY, 0f);
					isRotating = true;
				}
				else
				{
					transform.rotation = Quaternion.Euler(0f, targetY, 0f);
				}
			}

			return isRotating;
		}

	}
}