using FAS.Apartments;
using Zenject;

namespace FAS.Players.States.Zone
{
	public class LookTransition : TransitionZoneState
	{
		[Inject] private IApartmentWindow _apartmentWindow;

		protected override bool IsUseSelfCameraForOut { get; } = true;

		public override PlayerStates Key => PlayerStates.LookTransition;

		protected override void OnEnterToState()
		{
			Renderer.Hide();

			if (StateMachine.LastStateKey == PlayerStates.LookAtZone)
				MoveOutTransition();
			else
				MoveInTransition();
		}

		protected override void OnMoveInComplete()
		{
			base.OnMoveInComplete();

			if (_apartmentWindow.IsHasJumpscare)
			{
				UIScreensSwitcher.HideAll();
				_apartmentWindow.OnJumpscareShown += MoveOutTransition;
			}
			else
			{
				RequestTransition(PlayerStates.LookAtZone);
			}
		}
		
		protected override void OnMoveOutComplete()
		{
			base.OnMoveOutComplete();
			
			Renderer.Show();
			
			if (_apartmentWindow.IsHasJumpscare)
			{
				RequestTransition(PlayerStates.KnockedDown);
			}
			else
			{
				StateReturner.TryReturnLastControlledState();
				UIScreensSwitcher.ShowGameplayScreen();
			}
			
			TargetZone.Exit();
		}

		protected override void OnExitFromState()
		{
			_apartmentWindow.OnJumpscareShown -= MoveOutTransition;
			base.OnExitFromState();
		}
		
		protected override void OnFinishWaitingPhysicsUpdateAfterExit()
		{
			CameraBlendsChanger.TryChangeCameraBlends(CurrentZone.CameraBlends);
		}
	}
}