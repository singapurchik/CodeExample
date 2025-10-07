using System.Collections;
using Cinemachine;
using UnityEngine;
using VInspector;

namespace FAS
{
	[RequireComponent(typeof(CinemachineVirtualCameraBase))]
	public class CinemachineOnDisableTargetOverrider : MonoBehaviour
	{
		[SerializeField] private bool _isOverrideFollowTarget;
		[ShowIf(nameof(_isOverrideFollowTarget))]
		[SerializeField] private Transform _followTarget;
		[EndIf]
		[SerializeField] private bool _isOverrideAimTarget;
		[ShowIf(nameof(_isOverrideAimTarget))]
		[SerializeField] private Transform _aimTarget;
		[EndIf]
		
		private CinemachineVirtualCameraBase _camera;
		
		private Transform _defaultLookAtTarget;
		private Transform _defaultFollowTarget;

		private void Awake()
		{
			_camera = GetComponent<CinemachineVirtualCameraBase>();
			_defaultLookAtTarget = _camera.LookAt;
			_defaultFollowTarget = _camera.Follow;
		}

		private void OnEnable()
		{
			if (_isOverrideFollowTarget && _camera.Follow != _defaultFollowTarget)
				StartCoroutine(ReturnToDefaultFollowTarget());
			
			if (_isOverrideAimTarget && _camera.LookAt != _defaultLookAtTarget)
				StartCoroutine(ReturnToDefaultAimTarget());
		}

		private void OnDisable()
		{
			if (_isOverrideFollowTarget)
				_camera.Follow = _followTarget;
			
			if (_isOverrideAimTarget)
				_camera.LookAt = _aimTarget;
		}
		
		private IEnumerator ReturnToDefaultFollowTarget()
		{
			yield return null;
			_camera.Follow = _defaultFollowTarget;
		}
		
		private IEnumerator ReturnToDefaultAimTarget()
		{
			yield return null;
			_camera.LookAt = _defaultLookAtTarget;
		}
	}
}