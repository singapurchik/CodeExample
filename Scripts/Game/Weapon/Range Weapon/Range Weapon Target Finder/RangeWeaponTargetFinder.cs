using System;
using FAS.Players;
using UnityEngine;
using Zenject;

namespace FAS
{
	public class RangeWeaponTargetFinder : MonoBehaviour
	{
		[SerializeField] private float _horizontalInputThreshold = 0.5f;
		[SerializeField] private float _targetSwitchCooldown = 0.3f;
		[SerializeField] private LayerMask _visibilityMask;

		[Inject] private IReadOnlyCameraInputPanel _cameraInput;
		[Inject] private IPlayerCostumeProxy _costumeProxy;
		[Inject] private IZonesHolderInfo _zonesInfo;
		[Inject] private Camera _mainCamera;

		private static readonly RaycastHit[] _linecastBuffer = new RaycastHit[1];

		private float _nextAllowedSwitchTime;
		
		private bool _isFindRequested;
		private bool _isEnabled;

		private HumanoidBonesHolder Bones => _costumeProxy.Data.BonesHolder;
		public IRangeWeaponTarget CurrentTarget { get; private set; }
		
		public Vector3 CurrentTargetAimPosition => CurrentTarget.AimingPosition;

		public Vector3 CurrentTargetPosition => CurrentTarget.Position;
		
		public bool IsHasTarget => CurrentTarget != null;
		
		public event Action<IRangeWeaponTarget> OnTargetChanged;
		public event Action OnLoseTarget;
		
		public void SetEnabled(bool isEnabled)
		{
			if (_isEnabled != isEnabled)
			{
				if (_isEnabled)
				{
					_isFindRequested = false;
					AssignCurrentTarget(null);
				}

				_isEnabled = isEnabled;
			}
		}

		public void RequestFindClosestTarget()
		{
			_isFindRequested = true;
		}

		public void TrySwitchTargetByMouseDelta(float mouseDeltaX)
		{
			var isStrongEnough = Mathf.Abs(mouseDeltaX) > _horizontalInputThreshold;
			var isCooldownFinished = Time.timeSinceLevelLoad > _nextAllowedSwitchTime;

			if (_isEnabled && IsHasTarget && isStrongEnough && isCooldownFinished)
			{
				var candidate = FindBestSideTarget(mouseDeltaX, CurrentTarget);
				if (candidate != null)
				{
					AssignCurrentTarget(candidate);
					_nextAllowedSwitchTime = Time.timeSinceLevelLoad + _targetSwitchCooldown;
				}
			}
		}

		public void TryLoseTarget()
		{
			AssignCurrentTarget(null);
		}
		
		private void AssignCurrentTarget(IRangeWeaponTarget newTarget)
		{
			if (newTarget != CurrentTarget)
			{
				if (CurrentTarget != null)
					CurrentTarget.Deselect();

				CurrentTarget = newTarget;

				if (CurrentTarget != null)
				{
					CurrentTarget.Select();
					OnTargetChanged?.Invoke(CurrentTarget);
				}
				else
				{
					OnLoseTarget?.Invoke();
				}
			}
		}

		private bool IsTargetVisible(IRangeWeaponTarget target)
		{
			var result = false;

			var origin = Bones.Head.position;
			var destination = target.AimingPosition;
			var direction = destination - origin;
			var distance = direction.magnitude;

			if (distance > 0f)
			{
				var hits = Physics.RaycastNonAlloc(origin, direction / distance, _linecastBuffer, distance, _visibilityMask);
				result = hits == 0;
			}

			return result;
		}

		private IRangeWeaponTarget FindBestSideTarget(float inputX, IRangeWeaponTarget current)
		{
			var result = (IRangeWeaponTarget)null;

			var zone = _zonesInfo.CurrentZone;
			if (zone != null && current != null)
			{
				var desiredSign = inputX > 0f ? 1f : -1f;

				var currentDirection = current.Position - transform.position;
				currentDirection.y = 0f;

				var currentSqr = currentDirection.sqrMagnitude;
				if (currentSqr >= 0.000001f)
				{
					currentDirection *= 1f / Mathf.Sqrt(currentSqr);

					var list = zone.RangeWeaponTargets;
					var bestDot = -2f;

					for (int i = 0; i < list.Count; i++)
					{
						var candidate = list[i];

						var isCandidate = candidate != null && candidate != current;
						if (isCandidate)
						{
							var isVisible = IsTargetVisible(candidate);
							if (isVisible)
							{
								var toCandidate = candidate.Position - transform.position;
								toCandidate.y = 0f;

								var sq = toCandidate.sqrMagnitude;
								if (sq >= 0.000001f)
								{
									var normalized = toCandidate * (1f / Mathf.Sqrt(sq));

									var crossY = Vector3.Cross(currentDirection, normalized).y;
									var sideSign = Mathf.Sign(crossY);

									if (sideSign == desiredSign)
									{
										var dot = Vector3.Dot(currentDirection, normalized);
										var isBetter = dot > bestDot;

										if (isBetter)
										{
											bestDot = dot;
											result = candidate;
										}
									}
								}
							}
						}
					}
				}
			}

			return result;
		}

		private IRangeWeaponTarget FindClosestTargetAlongCameraForward()
		{
			var result = (IRangeWeaponTarget)null;

			var zone = _zonesInfo.CurrentZone;
			if (zone != null)
			{
				var forward = _mainCamera.transform.forward;
				forward.y = 0f;

				var forwardSqr = forward.sqrMagnitude;
				if (forwardSqr >= 0.000001f)
				{
					forward *= 1f / Mathf.Sqrt(forwardSqr);

					var list = zone.RangeWeaponTargets;
					var bestDot = -2f;

					for (int i = 0; i < list.Count; i++)
					{
						var candidate = list[i];

						if (candidate != null)
						{
							var isVisible = IsTargetVisible(candidate);
							if (isVisible)
							{
								var toCandidate = candidate.Position - transform.position;
								toCandidate.y = 0f;

								var sq = toCandidate.sqrMagnitude;
                                if (sq >= 0.000001f)
								{
									toCandidate *= 1f / Mathf.Sqrt(sq);

									var dot = Vector3.Dot(forward, toCandidate);
									var isBetter = dot > bestDot;

									if (isBetter)
									{
										bestDot = dot;
										result = candidate;
									}
								}
							}
						}
					}
				}
			}

			return result;
		}

		private void TryAutoSwitchByStick()
		{
			var isPressed = _cameraInput.IsInputProcess;
			var isStrongEnough = Mathf.Abs(_cameraInput.CurrentInputVector.x) > _horizontalInputThreshold;
			var isCooldownFinished = Time.timeSinceLevelLoad > _nextAllowedSwitchTime;

			if (_isEnabled && IsHasTarget && isPressed && isStrongEnough && isCooldownFinished)
			{
				var candidate = FindBestSideTarget(_cameraInput.CurrentInputVector.x, CurrentTarget);
				if (candidate != null)
				{
					AssignCurrentTarget(candidate);
					_nextAllowedSwitchTime = Time.timeSinceLevelLoad + _targetSwitchCooldown;
				}
			}
		}

		public void TrySelectClosestIfNone()
		{
			var hasZoneTargets = _zonesInfo.CurrentZone != null && _zonesInfo.CurrentZone.RangeWeaponTargets.Count > 0;

			if (hasZoneTargets)
			{
				if (!IsHasTarget)
				{
					var closest = FindClosestTargetAlongCameraForward();
					AssignCurrentTarget(closest);
				}
			}
			else
			{
				if (IsHasTarget)
					AssignCurrentTarget(null);
			}
		}

		public void MaintainSelectionWhileActive()
		{
			if (IsHasTarget)
			{
				var visible = IsTargetVisible(CurrentTarget);
				if (visible)
					TryAutoSwitchByStick();
				else
					AssignCurrentTarget(null);
			}
			else
			{
				TrySelectClosestIfNone();
			}
		}

		private void Update()
		{
			if (_isEnabled)
			{
				if (_isFindRequested)
					MaintainSelectionWhileActive();
				else
					TrySelectClosestIfNone();
				
				_isFindRequested = false;
			}
		}

#if UNITY_EDITOR
		[Header("DEBUG")]
		[SerializeField] private bool _showGizmos = true;

		private void OnDrawGizmos()
		{
			if (_showGizmos && Application.isPlaying)
			{
				var zone = _zonesInfo != null ? _zonesInfo.CurrentZone : null;
				var hasTargets = zone != null && zone.RangeWeaponTargets.Count > 0;

				if (hasTargets)
				{
					var origin = Bones.Head.position;
					var list = zone.RangeWeaponTargets;

					for (int i = 0; i < list.Count; i++)
					{
						var target = list[i];
						if (target != null)
						{
							var destination = target.AimingPosition;
							var direction = destination - origin;
							var distance = direction.magnitude;

							var blocked = Physics.Raycast(origin, distance > 0f ? direction / distance : Vector3.forward, distance, _visibilityMask);
							Gizmos.color = blocked ? Color.red : Color.green;
							Gizmos.DrawLine(origin, destination);

							Gizmos.color = target == CurrentTarget ? Color.yellow : Color.gray;
							Gizmos.DrawSphere(destination, 0.1f);
						}
					}
				}
			}
		}
#endif
	}
}
