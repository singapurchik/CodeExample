using UnityEngine;
using FAS.Sounds;
using Zenject;

namespace FAS.Corridor
{
	[RequireComponent(typeof(ZoneInside))]
	public class CorridorZone : Zone<ZoneInside>
	{
		[SerializeField] private Transform _enterPoint;
		[SerializeField] private float _interactableFindingRadius = 1f;
		
		[Inject] private IInteractableFinderRadius _interactableFinderRadius;
		[Inject] private CorridorScenarioPlayer _scenarioPlayer;
		[Inject] private IStreetAmbience _streetAmbience;
		[Inject] private IMusicChanger _musicChanger;

		public override Transform EnterPoint => _enterPoint;
		
		public override ZoneType Type => ZoneType.Corridor;
		
		public override bool IsReturnable { get; } = false;
		
		public override void Enter()
		{
			base.Enter();
			_musicChanger.PlayCorridorBackgroundMusic();
			_scenarioPlayer.TryChangeScenario(CorridorScenarioTypes.ExitFromCorridor);
			_scenarioPlayer.TryAssembleScenario();
			_streetAmbience.Mute();
		}

		public override void Exit()
		{
			base.Exit();
			_scenarioPlayer.TryDisassembleScenario();
		}

		private void Update()
		{
			if (IsActive)
				_interactableFinderRadius.RequestChangeRadius(_interactableFindingRadius);
		}
	}
}