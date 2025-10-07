using FAS.Players.Animations;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerMover : MonoBehaviour
	{
		[Inject] private CharacterController _characterController;
		[Inject] private PlayerAnimator _animator;
		[Inject] private PlayerRotator _rotator;
		[Inject] private IReadOnlyInput _input;
		[Inject] private PlayerJump _jump;

		private Vector3 _moveTransformRequestedPosition;
		private Vector3 _teleportRequestedPosition;
		private Vector3 _lastDeltaPosition;
		
		private float _moveCharacterControllerRequestedSpeed;
		private float _moveTransformRequestedSpeed;
		
		private bool _isCharacterControllerMovingRequested;
		private bool _isDisableRootMotionRequested;
		private bool _isTransformMovingRequested;
		private bool _isTeleportRequested;
		
		public void RequestDisableRootMotion() => _isDisableRootMotionRequested = true;

		public void RequestTeleportTo(Vector3 position)
		{
			_teleportRequestedPosition = position;
			_isTeleportRequested = true;
		}
		
		public void RequestMoveCharacterController(float speed)
		{
			_moveCharacterControllerRequestedSpeed = speed;
			_isCharacterControllerMovingRequested = true;
		}

		public void RequestTransformMove(Vector3 targetPosition, float speed)
		{
			_moveTransformRequestedPosition = targetPosition;
			_moveTransformRequestedSpeed = speed;
			_isTransformMovingRequested = true;
		}

		private void Teleport()
		{
			_characterController.enabled = false;
			transform.position = _teleportRequestedPosition;
		}

		private void MoveCharacterController()
			=> _characterController.Move(transform.forward * (_moveCharacterControllerRequestedSpeed * Time.deltaTime));

		private void MoveTransform()
		{
			_characterController.enabled = false;
			transform.position = Vector3.MoveTowards(transform.position, _moveTransformRequestedPosition, 
				_moveTransformRequestedSpeed * Time.deltaTime);
		}

		private void Update()
		{
			if (!_characterController.enabled)
				_characterController.enabled = true;
			
			if (_isTeleportRequested)
			{
				Teleport();
				_isTeleportRequested = false;
			}

			if (_isTransformMovingRequested)
				MoveTransform();
			
			if (_isCharacterControllerMovingRequested && _characterController.enabled)
				MoveCharacterController();
		}

		private void OnAnimatorMove()
		{
			if (!_isDisableRootMotionRequested && !_isCharacterControllerMovingRequested
			                                   && !_isTransformMovingRequested && _characterController.enabled)
			{
				_lastDeltaPosition = _animator.DeltaPosition;
				_characterController.Move(_lastDeltaPosition + _jump.VerticalVelocity);		
			}
			
			_isCharacterControllerMovingRequested = false;
			_isDisableRootMotionRequested = false;
			_isTransformMovingRequested = false;
		}
	}
}