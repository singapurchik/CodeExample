using VInspector;

namespace FAS.Corridor
{
	public class CorridorScenarioPlayer : ScenarioPlayer<CorridorScenarioTypes>
	{
#if UNITY_EDITOR
		[Button]
		private void FindScenarios()
		{
			ScenariosList.Clear();
			ScenariosList.AddRange(GetComponentsInChildren<Scenario<CorridorScenarioTypes>>(true));
		}
#endif
	}
}