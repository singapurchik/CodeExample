using UnityEngine;

namespace FAS.Players
{
	public interface ITriggerStatesSwitcher : IPlayerStateReturner
	{
		public void SwitchToMoveFromExitToStartCorridor(Transform startPoint, Transform finishPoint);
	}
}