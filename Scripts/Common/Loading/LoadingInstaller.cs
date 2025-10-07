using UnityEngine;
using VInspector;
using Zenject;

namespace FAS
{
	public class LoadingInstaller : MonoInstaller
	{
		[SerializeField] private LoadingScreen _loadingScreen;
		[SerializeField] private Loading _loading;
		[SerializeField] private Loader _loader;
	
		public override void InstallBindings()
		{
			Container.BindInstance(_loadingScreen).WhenInjectedIntoInstance(_loading);
			Container.BindInstance(_loading).WhenInjectedIntoInstance(_loader);
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_loadingScreen = GetComponentInChildren<LoadingScreen>(true);
			_loading = GetComponentInChildren<Loading>(true);
			_loader = GetComponentInChildren<Loader>(true);
		}
#endif
	}	
}