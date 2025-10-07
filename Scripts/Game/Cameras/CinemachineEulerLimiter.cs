using Cinemachine;
using UnityEngine;
using VInspector;

namespace FAS
{
	[ExecuteAlways]
	public sealed class CinemachineEulerLimiter : CinemachineExtension
	{
		[Header("Yaw (Y)")]
		[SerializeField] private bool _clampYaw = true;
		[ShowIf(nameof(_clampYaw))]
		[SerializeField] private bool _yawWrap;
		[EndIf]
		[Range(-180, 180)] [SerializeField] private float _minYaw = -60;
		[Range(-180, 180)] [SerializeField] private float _maxYaw = 60;

		[Header("Pitch (X)")]
		[SerializeField] private bool _clampPitch;
		[Range(-89, 89)] [SerializeField] private float _minPitch = -20;
		[Range(-89, 89)] [SerializeField] private float _maxPitch = 20;

		[Header("Roll (Z)")]
		[SerializeField] private bool _clampRoll;
		[ShowIf(nameof(_clampRoll))]
		[SerializeField] private bool _rollWrap;
		[EndIf]
		[Range(-180, 180)] [SerializeField] private float _minRoll = -10;
		[Range(-180, 180)] [SerializeField] private float _maxRoll = 10;

		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam,
			CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			if (stage != CinemachineCore.Stage.Aim) return;

			var e = state.RawOrientation.eulerAngles;
			var yaw = Mathf.DeltaAngle(0f, e.y);
			var pitch = Mathf.DeltaAngle(0f, e.x);
			var roll = Mathf.DeltaAngle(0f, e.z);

			if (_clampYaw)
				yaw = ClampSignedWrap(yaw, _minYaw, _maxYaw, _yawWrap);

			if (_clampPitch)
				pitch = ClampSigned(pitch, _minPitch, _maxPitch);

			if (_clampRoll)
				roll = ClampSignedWrap(roll, _minRoll, _maxRoll, _rollWrap);

			e.y = yaw;
			e.x = pitch;
			e.z = roll;
			state.RawOrientation = Quaternion.Euler(e);
		}

		private static float ClampSigned(float a, float min, float max)
		{
			a = Mathf.DeltaAngle(0f, a);
			min = Mathf.DeltaAngle(0f, min);
			max = Mathf.DeltaAngle(0f, max);
			if (max < min) (min, max) = (max, min);
			return Mathf.Clamp(a, min, max);
		}

		private static float ClampSignedWrap(float a, float min, float max, bool wrap)
		{
			a   = Mathf.DeltaAngle(0f, a);
			min = Mathf.DeltaAngle(0f, min);
			max = Mathf.DeltaAngle(0f, max);

			if (!wrap || Mathf.Approximately(min, max))
			{
				if (max < min) (min, max) = (max, min);
				return Mathf.Clamp(a, min, max);
			}

			bool inWrapRange = (min <= max) ? (a <= min || a >= max) : (a >= min || a <= max);
			if (inWrapRange) return a;

			float dMin = Mathf.Abs(Mathf.DeltaAngle(a, min));
			float dMax = Mathf.Abs(Mathf.DeltaAngle(a, max));
			return (dMin <= dMax) ? min : max;
		}

#if UNITY_EDITOR
		[Header("Gizmo (Editor only)")]
		public bool DrawGizmo = true;
		[Min(0.1f)] public float GizmoRadius = 2.5f;
		[Range(8, 128)] public int GizmoSegments = 64;
		public Color YawColor = new(0.20f, 0.80f, 1f, 0.85f);
		public Color PitchColor = new(0.20f, 0.80f, 1f, 0.85f);
		public Color RollColor = new(0.20f, 0.80f, 1f, 0.85f);
		public Color BoundColor = new(1f, 0.90f, 0.25f, 0.95f);
		public Color RefColor = new(0.25f, 1f, 0.25f, 0.85f);

		private void OnValidate()
		{
			if (!_yawWrap && _maxYaw < _minYaw) (_minYaw, _maxYaw) = (_maxYaw, _minYaw);
			if (_maxPitch < _minPitch) (_minPitch, _maxPitch) = (_maxPitch, _minPitch);
			if (!_rollWrap && _maxRoll < _minRoll) (_minRoll, _maxRoll) = (_maxRoll, _minRoll);

			_minYaw = Mathf.Clamp(_minYaw, -180, 180);
			_maxYaw = Mathf.Clamp(_maxYaw, -180, 180);
			_minPitch = Mathf.Clamp(_minPitch, -89, 89);
			_maxPitch = Mathf.Clamp(_maxPitch, -89, 89);
			_minRoll = Mathf.Clamp(_minRoll, -180, 180);
			_maxRoll = Mathf.Clamp(_maxRoll, -180, 180);

			GizmoSegments = Mathf.Clamp(GizmoSegments, 8, 128);
			GizmoRadius = Mathf.Max(0.1f, GizmoRadius);
		}

		private void OnDrawGizmosSelected()
		{
			if (!DrawGizmo) return;

			var origin = transform.position + Vector3.up * 0.05f;
			var baseFwd = Vector3.forward;
			var baseUp = Vector3.up;
			var baseRight = Vector3.right;

			Gizmos.color = RefColor;
			Gizmos.DrawLine(origin, origin + baseFwd * GizmoRadius);

			if (_clampYaw)
				DrawArcAroundAxis(origin, baseUp, baseFwd, GizmoRadius,
					_minYaw, _maxYaw, GizmoSegments, YawColor, BoundColor, _yawWrap);

			if (_clampPitch)
				DrawArcAroundAxis(origin, baseRight, baseFwd, GizmoRadius,
					_minPitch, _maxPitch, GizmoSegments, PitchColor, BoundColor);

			if (_clampRoll)
				DrawArcAroundAxis(origin, baseFwd, baseUp, GizmoRadius * 0.6f,
					_minRoll, _maxRoll, GizmoSegments, RollColor, BoundColor, _rollWrap);
		}

		private static void DrawArcAroundAxis(Vector3 origin, Vector3 axis, Vector3 startDir,
			float radius, float minDeg, float maxDeg, int seg, Color arcColor, Color boundColor, bool wrap = false)
		{
			Gizmos.color = boundColor;
			var d0 = Quaternion.AngleAxis(minDeg, axis) * startDir;
			var d1 = Quaternion.AngleAxis(maxDeg, axis) * startDir;
			Gizmos.DrawLine(origin, origin + d0 * radius);
			Gizmos.DrawLine(origin, origin + d1 * radius);

			Gizmos.color = arcColor;

			float len = wrap ? ((maxDeg - minDeg + 360f) % 360f)
			                 : Mathf.Abs(Mathf.DeltaAngle(minDeg, maxDeg));

			int steps = Mathf.Max(1, seg);
			Vector3 prev = origin + d0 * radius;
			for (int i = 1; i <= steps; i++)
			{
				float t = i / (float)steps;
				float a = minDeg + t * len;
				Vector3 dir = Quaternion.AngleAxis(a, axis) * startDir;
				Vector3 p = origin + dir * radius;
				Gizmos.DrawLine(prev, p);
				prev = p;
			}
		}
#endif
	}
}
