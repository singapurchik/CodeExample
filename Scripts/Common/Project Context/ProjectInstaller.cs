using UnityEngine.EventSystems;
using UnityEngine;
using VInspector;
using FAS.Scene;
using Zenject;

namespace FAS.Project
{
	public class ProjectInstaller : MonoInstaller
	{
		[SerializeField] private SceneCreator _sceneCreator;
		[SerializeField] private EventSystem _eventSystem;
		[SerializeField] private Loading _loading;
		[SerializeField] private Loader _loader;
		
		public override void InstallBindings()
		{
			InjectFromInstance();
			InjectIntoInstance();
			InjectInto();

			Container.Bind<GlobalInput>().AsSingle();
			Container.Bind<SignalBus>().AsSingle();
		}

		private void InjectFromInstance()
		{
			Container.Bind<IReadOnlyLoading>().FromInstance(_loading).AsSingle();
		}

		private void InjectIntoInstance()
		{
			Container.BindInstance(_loader).WhenInjectedIntoInstance(_sceneCreator);
		}
		
		private void InjectInto()
		{
			Container.BindInstance(_eventSystem).WhenInjectedInto<GlobalInput>();
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_sceneCreator = GetComponentInChildren<SceneCreator>(true);
			_eventSystem = GetComponentInChildren<EventSystem>(true);
			_loading = GetComponentInChildren<Loading>(true);
			_loader = GetComponentInChildren<Loader>(true);
		}
#endif
	}
}