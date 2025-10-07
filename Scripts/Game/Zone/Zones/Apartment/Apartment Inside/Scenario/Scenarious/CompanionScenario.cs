using FAS.Actors.Companion;
using UnityEngine;

namespace FAS.Apartments.Inside.Scenarios
{
	public class CompanionScenario : ScenarioWithSetActiveObjects<ApartmentInsideScenarioType>
	{
		[SerializeField] private Companion _companion;
		
		public override ApartmentInsideScenarioType Type => ApartmentInsideScenarioType.Companion;

		private void OnEnable()
		{
			_companion.OnInitialized += OnCompanionInitialize;
		}

		private void OnDisable()
		{
			_companion.OnInitialized -= OnCompanionInitialize;
		}

		private void OnCompanionInitialize()
		{
			DisableObjectsOnAssemble.Remove(_companion.gameObject);
			EnableObjectsOnAssemble.Remove(_companion.gameObject);
		}
	}
}