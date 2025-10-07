using VInspector;

namespace FAS.Apartments.Inside.Scenarios
{
	public class ApartmentInsideScenarioPlayer : ScenarioPlayer<ApartmentInsideScenarioType>
	{
#if UNITY_EDITOR
		[Button]
		private void FindScenarios()
		{
			ScenariosList.Clear();
			ScenariosList.AddRange(GetComponentsInChildren<Scenario<ApartmentInsideScenarioType>>(true));
		}
#endif
	}
}