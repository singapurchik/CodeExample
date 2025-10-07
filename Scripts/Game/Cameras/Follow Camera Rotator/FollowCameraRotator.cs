using Cinemachine;
using FAS.Players;
using UnityEngine;
using Zenject;
#if UNITY_EDITOR
using VInspector;
#endif

namespace FAS
{
	public class FollowCameraRotator : MonoBehaviour, IFollowCameraRotator
	{
		[Inject] private ICinemachineBrainInfo _cinemachineBrainInfo;
		[Inject] private IPlayerInfo _player;

		private CinemachineTransposer _transposer;
		private CinemachineVirtualCamera _camera;

		private enum CameraView
		{
			Back,
			Front,
			Left,
			Right
		}

		private float _nextUpdateTime;
		
		private const float UPDATE_INTERVAL = 0.5f;
		
		private void Awake()
		{
			_camera = GetComponent<CinemachineVirtualCamera>();
			_transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();

			SnapBack();
		}

		public void SnapFront() => TrySnap(CameraView.Front);

		public void SnapBack() => TrySnap(CameraView.Back);

		public void SnapRight() => TrySnap(CameraView.Right);

		public void SnapLeft() => TrySnap(CameraView.Left);

		private void TrySnap(CameraView view, float angleThreshold = 5f)
		{
			var angleOffset = view switch
			{
				CameraView.Front => 180f,
				CameraView.Left => 90f,
				CameraView.Right => -90f,
				_ => 0f
			};

			var localOffset = _transposer.m_FollowOffset;
			var desiredYaw = Quaternion.Euler(0f, _player.EulersAngles.y + angleOffset, 0f);
			var desiredPos = _player.Position + desiredYaw * localOffset;

			var toDesired = (_player.Position - desiredPos).normalized;
			var currentForward = _camera.transform.forward;
			float delta = Vector3.Angle(currentForward, toDesired);
			
			if (delta > angleThreshold)
			{
				var camRot = Quaternion.LookRotation(toDesired, Vector3.up);
				_camera.ForceCameraPosition(desiredPos, camRot);
				_camera.PreviousStateIsValid = false;	
			}
		}

		private void Update()
		{
			if (Time.timeSinceLevelLoad > _nextUpdateTime)
			{
				if (!_cinemachineBrainInfo.IsBlending() && _cinemachineBrainInfo.ActiveCamera != _camera)
				{
					switch (ActiveCameraData.OverrideFollowCameraSide)
					{
						case OverrideFollowCameraSide.Back:
							SnapBack();
							break;
						case OverrideFollowCameraSide.Front:
							SnapFront();
							break;
						case OverrideFollowCameraSide.Left:
							SnapLeft();
							break;
						case OverrideFollowCameraSide.Right:
							SnapRight();
							break;
						case OverrideFollowCameraSide.None:
						default:
							break;
					}
				}
				
				_nextUpdateTime = Time.timeSinceLevelLoad + UPDATE_INTERVAL;
			}
		}

#if UNITY_EDITOR
		[Button]
		public void SnapFrontEditor()
		{
			FindDependencies();
			SnapFront();
		}

		[Button]
		public void SnapBackEditor()
		{
			FindDependencies();
			SnapBack();
		}

		[Button]
		public void SnapRightEditor()
		{
			FindDependencies();
			SnapRight();
		}

		[Button]
		public void SnapLeftEditor()
		{
			FindDependencies();
			SnapLeft();
		}

		private void FindDependencies()
		{
			_camera = GetComponent<CinemachineVirtualCamera>();
			_transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
			_player = FindObjectOfType<Player>();
		}
#endif
	}
}