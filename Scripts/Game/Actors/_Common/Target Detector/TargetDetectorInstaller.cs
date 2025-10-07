using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Actors
{
	[RequireComponent(typeof(TargetDetector))]
	[RequireComponent(typeof(VisionSensor))]
	public class TargetDetectorInstaller : MonoInstaller
	{
		[SerializeField] private TargetDetectorView _targetDetectorView;
		[SerializeField] private TargetDetector _targetDetector;
		[SerializeField] private VisionSensor _visionSensor;

		public override void InstallBindings()
		{
			Container.BindInstance(_targetDetectorView).WhenInjectedIntoInstance(_targetDetector);
			Container.BindInstance(_visionSensor).WhenInjectedIntoInstance(_targetDetector);
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_targetDetectorView = transform.parent.GetComponentInChildren<TargetDetectorView>(true);
			_targetDetector = GetComponentInChildren<TargetDetector>(true);
			_visionSensor = GetComponentInChildren<VisionSensor>(true);
		}
#endif
	}
}