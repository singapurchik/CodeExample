using UnityEngine;

namespace FAS.Actors.Companion
{
	public class TeleportToPlayer : CompanionState
	{
		private float _finishTeleportTime;
		
		public override GirlStates Key => GirlStates.Teleport;

		private const float TELEPORT_DURATION = 0.1f;
		
		public override void Enter()
		{
			Mover.RequestTeleport(Companion.Owner.TeleportPosition);
			Rotator.RequestForceRotateHorizontal(Companion.Owner.TeleportRotation);
			Animator.ForcePlayIdleAnim();
			_finishTeleportTime = Time.timeSinceLevelLoad + TELEPORT_DURATION;
		}

		public override void Perform()
		{
			if (Time.timeSinceLevelLoad > _finishTeleportTime)
				RequestTransition(GirlStates.Idle);
		}
	}
}