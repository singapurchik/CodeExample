using UnityEngine;
using FAS.Players;
using Zenject;
using System;
using System.Collections;

namespace FAS.Actors
{
	[RequireComponent(typeof(SphereCollider))]
	public class VisionSensor : MonoBehaviour
	{
		[Header("Central Vision Zone")]
		[SerializeField] private float _visionAngle = 90f;
		[SerializeField] private float _visionDistance = 10f;
		[SerializeField] private float _detectionSpeed = 2f;
		[Header("Peripheral Vision Zone")]
		[SerializeField] private float _peripheralAngle = 60f;
		[SerializeField] private float _peripheralDetectionSpeed = 0.5f;
		[Header("Intuition Zone (Behind NPC)")]
		[SerializeField] private float _intuitionDistance = 4f;
		[SerializeField] private float _intuitionTimeToDetect = 2.5f;
		[SerializeField] private float _intuitionDetectionSpeed = 0.2f;
		[Header("Detection Decay")]
		[SerializeField] private float _detectionDecayDelay = 2f;
		[SerializeField] private float _detectionDecaySpeed = 1.5f;
		[Header("Distance Multiplier")]
		[SerializeField] private float _minDistanceMultiplier = 2f;
		[Header("Instant Detection")]
		[SerializeField] private float _instantDetectionDistance = 2f;
		[Header("System")]
		[SerializeField] private LayerMask _obstacleLayers;

		[Inject] private IPlayerInfo _player;
		
		private HumanoidBonesHolder BonesHolder => _player.Bones;
		private Coroutine _currentResetDetectionCoroutine;
		private SphereCollider _collider;

		private float _nextEyeContactCheckTime;
		private float _intuitionTimer;
		private float _lastSeenTime;

		private bool _isUpdateRequested;
		private bool _isHasVisualContact;

		private const float CROUCH_DETECTION_PROGRESS_MULTIPLIER = 0.4f;
		private const float CHECK_EYE_CONTACT_INTERVAL = 0.25f;
		private const float MAX_DISTANCE_MULTIPLIER = 1f;
		private const float HALF_ANGLE_MULTIPLIER = 0.5f;

		public ITargetPlayer CurrentTarget { get; private set; }

		public float DetectionProgressNormalized { get; private set; }

		public bool IsTargetInRange => CurrentTarget != null;
		public bool IsTargetDetected { get; private set; }
		public bool IsSeeTarget { get; private set; }

		public event Action<ITargetPlayer> OnTargetDetected;

		private void Awake()
		{
			_collider = GetComponent<SphereCollider>();
		}

		private void OnDisable()
		{
			ResetDetection();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out ITargetPlayer target))
				FindTarget(target);
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out ITargetPlayer target))
				LoseTarget();
		}

		private void FindTarget(ITargetPlayer target)
		{
			CurrentTarget = target;
			IsSeeTarget = HasFullVisionOnTarget();
			_nextEyeContactCheckTime = Time.timeSinceLevelLoad + CHECK_EYE_CONTACT_INTERVAL;
		}
		
		public void ResetDetectionNextFrame()
		{
			if (_currentResetDetectionCoroutine != null)
				StopCoroutine(_currentResetDetectionCoroutine);

			_currentResetDetectionCoroutine = StartCoroutine(ResetDetectionCoroutine());
		}

		private void ResetDetection()
		{
			LoseTarget();
			IsTargetDetected = false;
			DetectionProgressNormalized = 0f;
		}

		private IEnumerator ResetDetectionCoroutine()
		{
			_collider.enabled = false;
			ResetDetection();
			yield return null;
			_collider.enabled = true;
		}

		private void LoseTarget()
		{
			CurrentTarget = null;
			IsSeeTarget = false;
			_intuitionTimer = 0f;
		}

		private enum VisionZone
		{
			None,
			Central,
			Peripheral,
			Intuition
		}

		private VisionZone GetVisionZone(Vector3 targetPos)
		{
			var toTarget = targetPos - transform.position;
			var sqrDist = toTarget.sqrMagnitude;
			var angle = Vector3.Angle(transform.forward, toTarget);

			if (angle < _visionAngle * HALF_ANGLE_MULTIPLIER)
				return VisionZone.Central;

			if (angle < (_visionAngle + _peripheralAngle) * HALF_ANGLE_MULTIPLIER)
				return VisionZone.Peripheral;

			if (sqrDist <= _intuitionDistance * _intuitionDistance)
				return VisionZone.Intuition;

			return VisionZone.None;
		}

		private bool HasFullVisionOnTarget()
		{
			if (!IsTargetInRange)
				return false;

			bool isUnobstructed(Vector3 point) =>
				!Physics.Linecast(BonesHolder.Head.position, point, _obstacleLayers);

			var targetBones = CurrentTarget.Bones;
			return isUnobstructed(targetBones.Head.position) && isUnobstructed(targetBones.Hips.position);
		}

		private float GetDistanceMultiplier()
		{
			if (IsTargetInRange)
			{
				var sqrDistance = Vector3.SqrMagnitude(transform.position - CurrentTarget.Position);
				var minSqr = _instantDetectionDistance * _instantDetectionDistance;
				var maxSqr = _visionDistance * _visionDistance;

				return Mathf.Lerp(
					_minDistanceMultiplier, MAX_DISTANCE_MULTIPLIER, Mathf.InverseLerp(minSqr, maxSqr, sqrDistance));
			}

			return MAX_DISTANCE_MULTIPLIER;
		}

		private void TrySetTargetDetected()
		{
			if (!IsTargetDetected)
			{
				IsTargetDetected = true;
				OnTargetDetected?.Invoke(CurrentTarget);	
			}
		}

		private void UpdateVision()
		{
			if (Time.timeSinceLevelLoad > _nextEyeContactCheckTime)
			{
				_isHasVisualContact = HasFullVisionOnTarget();
				_nextEyeContactCheckTime = Time.timeSinceLevelLoad + CHECK_EYE_CONTACT_INTERVAL;

				if (_isHasVisualContact)
					_lastSeenTime = Time.timeSinceLevelLoad;
			}

			bool isInFront = Vector3.Angle(transform.forward, CurrentTarget.Position - transform.position)
			                 <= (_visionAngle + _peripheralAngle) * 0.5f;
			
			bool isInstantDetected = _isHasVisualContact && isInFront &&
			                         Vector3.SqrMagnitude(transform.position - CurrentTarget.Position)
			                         <= _instantDetectionDistance * _instantDetectionDistance;

			if (isInstantDetected)
			{
				DetectionProgressNormalized = 1f;
				IsSeeTarget = true;
				TrySetTargetDetected();
			}
			else
			{
				var zone = GetVisionZone(CurrentTarget.Position);

				switch (zone)
				{
					case VisionZone.Central:
					case VisionZone.Peripheral:
						_intuitionTimer = 0f;
						if (_isHasVisualContact)
							IncreaseDetection();
						else
							TryStartDecay();
						break;

					case VisionZone.Intuition:
						if (_isHasVisualContact)
						{
							_intuitionTimer += Time.deltaTime;
							if (_intuitionTimer >= _intuitionTimeToDetect)
								IncreaseDetection();
							else
								TryStartDecay();
						}
						else
						{
							_intuitionTimer = 0f;
							TryStartDecay();
						}
						break;

					default:
						_intuitionTimer = 0f;
						TryStartDecay();
						break;
				}

				IsSeeTarget = DetectionProgressNormalized >= 1f;
			}
		}

		private void TryStartDecay()
		{
			if (Time.timeSinceLevelLoad - _lastSeenTime >= _detectionDecayDelay)
				DecreaseDetection();
		}

		private void IncreaseDetection()
		{
			var speed = GetVisionZone(CurrentTarget.Position) switch
			{
				VisionZone.Central => _detectionSpeed,
				VisionZone.Peripheral => _peripheralDetectionSpeed,
				VisionZone.Intuition => _intuitionDetectionSpeed,
				_ => 0f
			};

			if (speed > 0f)
			{
				float progressMultiplier = 1;

				if (CurrentTarget.IsCrouched)
					progressMultiplier = CROUCH_DETECTION_PROGRESS_MULTIPLIER;

				DetectionProgressNormalized = Mathf.MoveTowards(
					DetectionProgressNormalized, 1f,
					speed * progressMultiplier * GetDistanceMultiplier() * Time.deltaTime);
			}

			if (DetectionProgressNormalized >= 1f)
				TrySetTargetDetected();
		}


		private void DecreaseDetection()
		{
			DetectionProgressNormalized = Mathf.MoveTowards(
				DetectionProgressNormalized, 0f, _detectionDecaySpeed * Time.deltaTime);
		}

		private void DecayDetection()
		{
			if (DetectionProgressNormalized > 0f)
				DetectionProgressNormalized = Mathf.MoveTowards(
					DetectionProgressNormalized, 0f, _detectionDecaySpeed * Time.deltaTime);

			IsSeeTarget = false;
		}
		
		public void RequestUpdate() => _isUpdateRequested = true;

		private void Update()
		{
			if (_isUpdateRequested && !IsTargetDetected)
			{
				if (IsTargetInRange)
					UpdateVision();
				else
					DecayDetection();
			}

			_isUpdateRequested = false;
		}

#if UNITY_EDITOR
		[Header("Debug")]
		[SerializeField] private bool _showGizmos;

		private static Mesh _sectorMesh;

		private static Mesh GenerateSectorMesh(float angleDegrees, float radius, int segments = 30)
		{
			Mesh mesh = new Mesh();

			int vertCount = segments + 2;
			Vector3[] vertices = new Vector3[vertCount];
			int[] triangles = new int[segments * 3];

			vertices[0] = Vector3.zero;

			for (int i = 0; i <= segments; i++)
			{
				float angle = Mathf.Deg2Rad * (i / (float)segments * angleDegrees - angleDegrees / 2f);
				vertices[i + 1] = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle)) * radius;
			}

			for (int i = 0; i < segments; i++)
			{
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}

			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.RecalculateNormals();
			return mesh;
		}

		private void OnDrawGizmos()
		{
			if (!_showGizmos)
				return;

			Vector3 origin = transform.position;
			Quaternion forwardRot = Quaternion.LookRotation(transform.forward, Vector3.up);

			if (_sectorMesh == null || _sectorMesh.vertexCount != 32)
				_sectorMesh = GenerateSectorMesh(150f, _visionDistance);

			Gizmos.color = (CurrentTarget != null && GetVisionZone(CurrentTarget.Position) == VisionZone.Central)
				? Color.red
				: Color.gray;
			Gizmos.DrawMesh(
				GenerateSectorMesh(_visionAngle, _visionDistance),
				origin,
				forwardRot
			);

			Gizmos.color = (CurrentTarget != null && GetVisionZone(CurrentTarget.Position) == VisionZone.Peripheral)
				? new Color(1f, 0.5f, 0f, 0.15f)
				: new Color(1f, 1f, 1f, 0.15f);
			Gizmos.DrawMesh(
				GenerateSectorMesh(_peripheralAngle * 0.5f, _visionDistance),
				origin,
				forwardRot * Quaternion.Euler(0f, -(_visionAngle * 0.5f + _peripheralAngle * 0.25f), 0f)
			);

			Gizmos.color = (CurrentTarget != null && GetVisionZone(CurrentTarget.Position) == VisionZone.Peripheral)
				? new Color(1f, 0.5f, 0f, 0.15f)
				: new Color(1f, 1f, 1f, 0.15f);
			Gizmos.DrawMesh(
				GenerateSectorMesh(_peripheralAngle * 0.5f, _visionDistance),
				origin,
				forwardRot * Quaternion.Euler(0f, (_visionAngle * 0.5f + _peripheralAngle * 0.25f), 0f)
			);

			float intuitionAngle = 360f - _visionAngle - _peripheralAngle;
			if (intuitionAngle > 0f)
			{
				Gizmos.color = (CurrentTarget != null && GetVisionZone(CurrentTarget.Position) == VisionZone.Intuition)
					? Color.red
					: new Color(0.6f, 0.4f, 0.2f, 0.8f);
				Gizmos.DrawMesh(
					GenerateSectorMesh(intuitionAngle, _intuitionDistance),
					origin,
					forwardRot * Quaternion.Euler(0f, 180f, 0f)
				);
			}

			Gizmos.color = (CurrentTarget != null &&
			                (GetVisionZone(CurrentTarget.Position) == VisionZone.Central ||
			                 GetVisionZone(CurrentTarget.Position) == VisionZone.Peripheral) &&
			                Vector3.SqrMagnitude(transform.position - CurrentTarget.Position) <=
			                _instantDetectionDistance * _instantDetectionDistance)
				? Color.red
				: new Color(1f, 1f, 0f, 0.4f);
			Gizmos.DrawMesh(
				GenerateSectorMesh(_visionAngle + _peripheralAngle, _instantDetectionDistance),
				origin,
				forwardRot
			);

			if (CurrentTarget != null && BonesHolder != null)
			{
				Vector3 eye = BonesHolder.Head.position;
				var targetBones = CurrentTarget.Bones;
				Vector3 head = targetBones.Head.position;
				Vector3 hips = targetBones.Hips.position;

				Gizmos.color = !Physics.Linecast(eye, head, _obstacleLayers) ? Color.green : Color.red;
				Gizmos.DrawLine(eye, head);

				Gizmos.color = !Physics.Linecast(eye, hips, _obstacleLayers) ? Color.green : Color.red;
				Gizmos.DrawLine(eye, hips);
			}
		}
#endif
	}
}