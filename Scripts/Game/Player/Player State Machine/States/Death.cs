namespace FAS.Players.States
{
	public class Death : PlayerState
	{
		public override PlayerStates Key => PlayerStates.Death;
		
		private bool _isWaitingForCameraBlend;
		
		public override bool IsPlayerControlledState => false;
		
		public override void Enter()
		{
			_isWaitingForCameraBlend = true;
			UIScreensSwitcher.HideAll();
			InputControl.DisableMovementInput();
			InputControl.DisableButtonsInput();
		}
		
		public override void Perform()
		{
			if (_isWaitingForCameraBlend && !CinemachineBrainInfo.IsBlending())
			{
				_isWaitingForCameraBlend = false;
			}
		}
	}
}