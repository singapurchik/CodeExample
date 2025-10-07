using UnityEngine.Serialization;
using Cinemachine;
using UnityEngine;
using FAS.Players;
using Zenject;
using System;

namespace FAS
{
	[RequireComponent(typeof(CinemachineVirtualCamera))]
	public class CinemachineCameraTargetSetter : MonoBehaviour
	{
		[Serializable]
		public enum CameraTargetType
		{
			Player,
			PlayerBone
		}

		[Serializable]
		public struct CameraTargets
		{
			[SerializeField] private bool _isHasAimTarget;
			[SerializeField] private CameraTargetType _aimTarget;
			[SerializeField] private HumanoidBoneType _aimTargetBone;

			[FormerlySerializedAs("_isHasBodyTarget")]
			[SerializeField] private bool _isHasFollowTarget;

			[FormerlySerializedAs("_bodyTarget")]
			[SerializeField] private CameraTargetType _followTarget;

			[FormerlySerializedAs("_bodyTargetBone")]
			[SerializeField] private HumanoidBoneType _followTargetBone;

			public bool IsHasFollowTarget => _isHasFollowTarget;
			public bool IsHasAimTarget => _isHasAimTarget;

			public CameraTargetType FollowTarget => _followTarget;
			public CameraTargetType AimTarget => _aimTarget;

			public HumanoidBoneType FollowTargetBone => _followTargetBone;
			public HumanoidBoneType AimTargetBone => _aimTargetBone;
		}

		[SerializeField] private CameraTargets _targets;

		[Inject] private ICinemachineCameraTarget _cameraTarget;
		[Inject] private IPlayerCostumeProxy _costumeProxy;

		private CinemachineVirtualCamera _camera;

		private void Awake()
		{
			_camera = GetComponent<CinemachineVirtualCamera>();
			UpdateTargets();
			_costumeProxy.OnCostumeChanged += OnPlayerCostumeChanged;
			enabled = false;
		}

		private void OnDestroy()
		{
			_costumeProxy.OnCostumeChanged -= OnPlayerCostumeChanged;
		}

		private void OnPlayerCostumeChanged(IPlayerCostumeData costumeData) => UpdateTargets();

		private void UpdateTargets()
		{
			if (_targets.IsHasFollowTarget)
				_camera.Follow = GetTarget(_targets.FollowTarget, _targets.FollowTargetBone);

			if (_targets.IsHasAimTarget)
				_camera.LookAt = GetTarget(_targets.AimTarget, _targets.AimTargetBone);
		}

		private Transform GetTarget(CameraTargetType type, HumanoidBoneType boneType)
		{
			Transform target = null;

			switch (type)
			{
				case CameraTargetType.Player:
					target = _cameraTarget.PlayerTransform;
					break;
				case CameraTargetType.PlayerBone:
					target = _cameraTarget.Bones.GetBoneByType(boneType);
					break;
			}

			return target;
		}
	}
}