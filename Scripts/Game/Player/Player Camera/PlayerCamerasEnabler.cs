using Cinemachine;
using UnityEngine;
using FAS.Utils;
using Zenject;

namespace FAS.Players
{
	public class PlayerCamerasEnabler : MonoBehaviour
	{
		[Inject (Id = PlayerCameraType.RangeWeaponAim)] private CinemachineVirtualCamera _rangeWeaponAimCamera;
		[Inject (Id = PlayerCameraType.Follow)] private CinemachineVirtualCamera _followCamera;
		[Inject] private FatalityCamera _fatalityCamera;
		[Inject] private DeathCamera _deathCamera;

		public void EnableFatalityCamera(Transform target)
		{
			_fatalityCamera.AddTarget(target);
			_fatalityCamera.gameObject.TryEnable();
		}

		public void DisableRangeWeaponAimCamera() => _rangeWeaponAimCamera.gameObject.TryDisable();
		
		public void EnableRangeWeaponAimCamera() => _rangeWeaponAimCamera.gameObject.TryEnable();
		
		public void DisableFollowCamera() => _followCamera.gameObject.TryDisable();
		
		public void EnableDeathCamera() => _fatalityCamera.gameObject.TryEnable();
		
		public void EnableFollowCamera() => _followCamera.gameObject.TryEnable();
	}
}