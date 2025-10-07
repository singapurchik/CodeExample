using FAS.Triggers.Corridor;
using FAS.Triggers;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerTriggerVisitor : MonoBehaviour, ITriggerVisitor
	{
		[Inject] private ITriggerStatesSwitcher _statesSwitcher;
		
		public void Apply(MoveFromExitToStartCorridor moveFromExitToStartCorridor)
		{
			_statesSwitcher.SwitchToMoveFromExitToStartCorridor(
				moveFromExitToStartCorridor.StartPoint, moveFromExitToStartCorridor.FinishPoint);
		}
	}
}