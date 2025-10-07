using UnityEngine;

namespace FAS.Players.States.Zone
{
	public class TeleportTransition : TransitionZoneState
	{
		private Vector3 _lastTransitionPosition;
		
		private float _lastTransitionRotationY;

		private bool _isCameraMoveIn;
		
		protected override bool IsUseSelfCameraForOut { get; } = false;
		
		public override PlayerStates Key => PlayerStates.TeleportTransition;
		
		protected override void OnStartTransition()
		{
			SoundEffectsPlayer.PlayOneShot(TransitionData.StartSound);
			base.OnStartTransition();
		}

		protected override void MoveInTransition()
		{
			if (CurrentZone.IsReturnable)
			{
				_lastTransitionRotationY = transform.eulerAngles.y;
				_lastTransitionPosition = transform.position;
			}

			base.MoveInTransition();
		}
		
		protected override void OnMoveOutComplete()
		{
			CamerasEnabler.DisableFollowCamera();
			
			if (IsUseSelfCameraForOut)
				CurrentCamera.OnExitFinished.RemoveListener(OnMoveOutComplete);
			else
				LastCamera.OnExitFinished.RemoveListener(OnMoveOutComplete);
			
			FinishTransition(_lastTransitionPosition, _lastTransitionRotationY + 180);
		}

		protected override void OnMoveInComplete()
		{
			CamerasEnabler.DisableFollowCamera();
			base.OnMoveInComplete();
			FinishTransition(TargetZone.EnterPoint.position, TargetZone.EnterPoint.eulerAngles.y);
		}
		
		private void FinishTransition(Vector3 targetPosition, float targetRotationY)
		{
			Renderer.Show();
			TargetZone.Enter();
			SoundEffectsPlayer.PlayOneShot(TransitionData.FinishSound);
			Mover.RequestTeleportTo(targetPosition);
			Rotator.ForceRotateHorizontal(targetRotationY);
			StateReturner.TryReturnLastControlledState();
			UIScreensSwitcher.ShowGameplayScreen();
		}
		
		protected override void OnExitFromState()
		{
			if (CurrentZone != TargetZone)
				CurrentZone.Exit();
			
			UIScreensSwitcher.ShowGameplayScreen();
			base.OnExitFromState();
		}
		
		protected override void OnFinishWaitingPhysicsUpdateAfterExit()
		{
			base.OnFinishWaitingPhysicsUpdateAfterExit();

			if (TransitionData.CameraMoveType == Transitions.CameraMoveType.In)
			{
				CurrentCamera.Disable();
			}
			else
			{
				if (IsUseSelfCameraForOut)
					CurrentCamera.Disable();
				else
					LastCamera?.Disable();
			}
			
			CamerasEnabler.EnableFollowCamera();
		}
	}
}