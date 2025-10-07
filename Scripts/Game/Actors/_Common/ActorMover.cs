using UnityEngine.AI;
using UnityEngine;
using Zenject;

namespace FAS.Actors
{
	public abstract class ActorMover : MonoBehaviour
	{
		[field: SerializeField] public Transform Body { get; private set; }
		[SerializeField] private float _targetRepathThreshold = 0.25f;
		[SerializeField] private float _selfRepathThreshold = 0.25f;

		[Inject] protected ActorAnimator Animator;
		[Inject] private IPausableInfo _pausable;
		[Inject] private NavMeshAgent _agent;

		private NavMeshPath _navMeshPath;

		private Vector3 _lastSetDestination = new (float.PositiveInfinity, 0, 0);
		private Vector3 _transformMoveTargetPosition;
		private Vector3 _teleportTargetPosition;
		private Vector3 _navMeshTargetPosition;
		
		private float _transformMoveSpeed;
		private float _nextPathUpdateTime;
		
		private bool _isAgentPositionUpdatingInRootMotionRequested;
		private bool _isDisableNavMeshAgentRequested;
		private bool _isRootMotionMovedLastFrame;
		private bool _isTransformMoveThisFrame;
		private bool _isTransformMoveRequested;
		private bool _isRootMotionRequested;
		private bool _isTeleportRequested;
		private bool _isDisableRequested;
		private bool _isMovedLastFrame;
		
		private const float MIN_REMAINING_DISTANCE = 0.1f;
		private const float PATH_UPDATE_INTERVAL = 0.2f;

		public bool IsProcessMovement => _agent.enabled && (_isTransformMoveThisFrame || _isRootMotionMovedLastFrame
		                                 || _agent.pathPending
		                                 || _agent.remainingDistance > MIN_REMAINING_DISTANCE + _agent.stoppingDistance);
		public bool IsFinishMoveThisFrame => !IsProcessMovement && IsMovedLastFrame;
		public bool IsMovingToTarget { get; private set; }
		public bool IsMovedLastFrame { get; private set; }
		
		public float StoppingSqrMagnitude => _agent.stoppingDistance * _agent.stoppingDistance;
		
		private void Awake()
		{
			_navMeshPath = new NavMeshPath();
			_agent.autoRepath = false;
		}

		public void RequestDisableNavMeshAgent() => _isDisableNavMeshAgentRequested = true;
		
		public void RequestDisable() => _isDisableRequested = true;
		
		public void SetStoppingDistance(float stoppingDistance) => _agent.stoppingDistance = stoppingDistance;

		public void RequestEnableRootMotion(bool isAgentPositionUpdatingInRootMotionRequested = true)
		{
			_isAgentPositionUpdatingInRootMotionRequested = isAgentPositionUpdatingInRootMotionRequested;
			_isRootMotionRequested = true;
		}

		private bool NeedRepath(Vector3 target)
		{
			if (_agent.pathPending) return false;
			if (!_agent.hasPath || _agent.pathStatus != NavMeshPathStatus.PathComplete) return true;
			if ((_lastSetDestination - target).sqrMagnitude >= _targetRepathThreshold * _targetRepathThreshold) return true;
			return false;
		}

		private bool CalculatePath(Vector3 target)
		{
			IsMovingToTarget = false;

			if (_navMeshTargetPosition != target)
			{
				_navMeshTargetPosition = target;

				if (!_agent.CalculatePath(target, _navMeshPath))
					return IsMovingToTarget;
			}

			IsMovingToTarget = _navMeshPath.status == NavMeshPathStatus.PathComplete && _navMeshPath.corners.Length > 0;
			return IsMovingToTarget;
		}

		public void NavMeshMove(Vector3 target)
		{
			_agent.enabled = true;
			_agent.updatePosition = true;
			_isDisableRequested = false;

			if (Time.timeSinceLevelLoad > _nextPathUpdateTime)
			{
				_nextPathUpdateTime = Time.timeSinceLevelLoad + PATH_UPDATE_INTERVAL;

				if (NeedRepath(target))
				{
					_agent.isStopped = false;

					if (CalculatePath(target))
					{
						_agent.SetDestination(_navMeshTargetPosition);
						_lastSetDestination = _navMeshTargetPosition;
					}
					else
					{
						if (_navMeshPath.corners != null && _navMeshPath.corners.Length > 0)
						{
							var fallback = _navMeshPath.corners[^1];
							_agent.SetDestination(fallback);
							_lastSetDestination = fallback;
						}
						else
						{
							_agent.ResetPath();
							_lastSetDestination = new Vector3(float.PositiveInfinity, 0f, 0f);
						}
					}
				}
			}
		}
		
		public void TryStopMove()
		{
			if (_agent.enabled && !_isDisableRequested)
			{
				_agent.isStopped = true;
				_agent.ResetPath();
			}
		}

		public void RequestTeleport(Vector3 position)
		{
			_isTeleportRequested = true;
			_teleportTargetPosition = position;
		}

		public void RequestTransformMove(Vector3 target, float speed)
		{
			_isTransformMoveRequested = true;
			_transformMoveTargetPosition = target;
			_transformMoveSpeed = speed;
			
			_isTransformMoveThisFrame =
				Vector3.SqrMagnitude(transform.position - _transformMoveTargetPosition) > 0.1f;
		}
		
		private void Teleport()
		{
			_agent.Warp(_teleportTargetPosition);
		}

		private void TransformMove()
		{
			var newPos = Vector3.MoveTowards(
				transform.position, _transformMoveTargetPosition, 
				_transformMoveSpeed * Time.deltaTime);
			
			transform.position = newPos;
			_agent.updatePosition = false;
			_agent.nextPosition = newPos;
		}
		
		private void RootMotionMove()
		{
			_agent.speed = 0;
			var newPos = transform.position + Animator.DeltaPosition;
			transform.position = newPos;
			transform.rotation *= Animator.DeltaRotation;
			_agent.updatePosition = _isAgentPositionUpdatingInRootMotionRequested;
			_agent.nextPosition = newPos;
			_isRootMotionMovedLastFrame = Animator.DeltaPosition != Vector3.zero;
		}
		
		private void Update()
		{
			if (_pausable.IsPaused) return;
			
			if (_isDisableRequested)
			{
				_agent.enabled = false;
				_isDisableRequested = false;
			}
			else
			{
				if (_isDisableNavMeshAgentRequested)
				{
					if (_agent.enabled)
						_agent.enabled = false;
				}
				else if (_agent.enabled == false)
				{
					_agent.enabled = true;
				}

				if (_isTeleportRequested)
					Teleport();
				else if (_isTransformMoveRequested)
					TransformMove();
				else if (_agent.enabled)
					_agent.updatePosition = true;
			}

			_isDisableNavMeshAgentRequested = false;
			_isTransformMoveRequested = false;
			_isTeleportRequested = false;
		}
		
		private void OnAnimatorMove()
		{
			_isRootMotionMovedLastFrame = false;
			
			if (_isRootMotionRequested)
				RootMotionMove();
			else if (_agent.enabled)
				_agent.speed = Animator.Velocity.magnitude;
			
			_isAgentPositionUpdatingInRootMotionRequested = false;
			_isRootMotionRequested = false;
		}
		
		private void LateUpdate()
		{
			_isTransformMoveThisFrame = false;
			_isMovedLastFrame = IsMovedLastFrame;
			IsMovedLastFrame = _isMovedLastFrame || IsProcessMovement;
		}
	}
}