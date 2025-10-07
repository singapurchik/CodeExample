using System.Collections.Generic;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Corridor
{
	public class CorridorInstaller : MonoInstaller
	{
		[SerializeField] private CorridorExitScenario _corridorExitScenario;
		[SerializeField] private CorridorScenarioPlayer _scenarioPlayer;
		[SerializeField] private CorridorZone _corridor;
		[SerializeField] private List<CorridorDoor> _doors = new ();
		
		public override void InstallBindings()
		{
			Container.BindInstance(_doors).WhenInjectedIntoInstance(_corridorExitScenario);
			Container.BindInstance(_scenarioPlayer).WhenInjectedIntoInstance(_corridor);
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_doors.Clear();
			_doors.AddRange(GetComponentsInChildren<CorridorDoor>(true));
			
			_corridorExitScenario = GetComponentInChildren<CorridorExitScenario>(true);
			_scenarioPlayer = GetComponentInChildren<CorridorScenarioPlayer>(true);
			_corridor = GetComponentInChildren<CorridorZone>(true);
		}
#endif
	}
}