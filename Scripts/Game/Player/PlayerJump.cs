using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerJump : MonoBehaviour
	{
		[SerializeField] private float _jumpHeight = 1.2f;
		[SerializeField] private float _gravity = -15.0f;
		[SerializeField] private float _jumpTimeout = 0.50f;
		[SerializeField] private float _jumpDelay = 0.1f;
		
		[Inject] private IGroundChecker _groundChecker;

		private float _verticalVelocity;
		private float _fallTimeoutDelta;
		private float _jumpDelayTimer;
		
		private bool _isWaitingForJump;

		private const float TERMINAL_VELOCITY = 53.0f;
		
		public Vector3 VerticalVelocity => new (0.0f, _verticalVelocity, 0.0f);

		private void FreeFall()
		{
			if (_fallTimeoutDelta >= 0.0f)
				_fallTimeoutDelta -= Time.deltaTime;
		}

		private void CheckAndRestVelocity()
		{
			if (_verticalVelocity < 0)
				_verticalVelocity = -2f;
		}

		private void ApplyGravity()
		{
			if (_verticalVelocity < TERMINAL_VELOCITY)
				_verticalVelocity += _gravity * Time.deltaTime;
		}
		
		public void UpdateGravity()
		{
			if (_groundChecker.IsGrounded)
				CheckAndRestVelocity();
			else
				FreeFall();
			
			ApplyGravity();
		}
	}
}