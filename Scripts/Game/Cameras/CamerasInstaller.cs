using System.Collections.Generic;
using Cinemachine;
using FAS.Players;
using FAS.Players.States;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS
{
	public class CamerasInstaller : MonoInstaller
	{
		[SerializeField] private List<ActiveCameraListener> _cameraListeners = new ();
		[SerializeField] private CinemachineBrainWrapper _brainWrapper;
		[SerializeField] private Player _player;
		
		public override void InstallBindings()
		{
			Container.Bind<ICinemachineBrainInfo>().FromInstance(_brainWrapper).AsSingle();
			
			BindCameraBlendsChangeInto<ActiveCameraData>();
			BindCameraBlendsChangeInto<PlayerState>();
			BindCameraBlendsChangeInto<Zones>();
			
			Container.Bind<ICinemachineCameraTarget>().FromInstance(_player)
				.WhenInjectedInto<CinemachineCameraTargetSetter>();
		}

		private void BindCameraBlendsChangeInto<T>()
			=> Container.Bind<ICameraBlendsChanger>().FromInstance(_brainWrapper).WhenInjectedInto<T>();

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_brainWrapper = FindObjectOfType<CinemachineBrainWrapper>(true);
			_player = FindObjectOfType<Player>(true);
			
			_cameraListeners.Clear();
			_cameraListeners.AddRange(FindObjectsOfType<ActiveCameraListener>(true));
		}
#endif
	}
}