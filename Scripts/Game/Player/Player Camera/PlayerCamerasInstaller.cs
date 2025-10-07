using Cinemachine;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Players
{
	public class PlayerCamerasInstaller : MonoInstaller
	{
		[SerializeField] private CinemachineVirtualCamera _rangeWeaponAimCamera;
		[SerializeField] private FollowCameraRotator _followCameraRotator;
		[SerializeField] private CinemachineVirtualCamera _followCamera;
		[SerializeField] private CinemachineTargetGroup _targetGroup;
		[SerializeField] private FatalityCamera _fatalityCamera;
		[SerializeField] private DeathCamera _deathCamera;
		
		public override void InstallBindings()
		{
			Container.Bind<IFollowCameraRotator>().FromInstance(_followCameraRotator).AsSingle();
			
			Container.BindInstance(_rangeWeaponAimCamera).WithId(PlayerCameraType.RangeWeaponAim)
				.WhenInjectedInto<PlayerCamerasEnabler>();
			Container.BindInstance(_followCamera).WithId(PlayerCameraType.Follow)
				.WhenInjectedInto<PlayerCamerasEnabler>();

			Container.BindInstance(_fatalityCamera).WhenInjectedInto<PlayerCamerasEnabler>();
			Container.BindInstance(_deathCamera).WhenInjectedInto<PlayerCamerasEnabler>();
			Container.BindInstance(_targetGroup).WhenInjectedInto<FatalityCamera>();
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_followCameraRotator = GetComponentInChildren<FollowCameraRotator>(true);
			_targetGroup = GetComponentInChildren<CinemachineTargetGroup>(true);
			_fatalityCamera = GetComponentInChildren<FatalityCamera>(true);
			_deathCamera = GetComponentInChildren<DeathCamera>(true);
		}
#endif
	}
}