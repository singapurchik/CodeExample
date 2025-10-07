namespace FAS.Players.States
{
	public class Respawn : PlayerState
	{
		public override PlayerStates Key => PlayerStates.Respawn;
		
		public override bool IsPlayerControlledState => false;

		public override void Enter()
		{
			CharacterController.enabled = false;
			Transform.position = Spawnable.SpawnPosition;
			RequestTransition(PlayerStates.Free);
		}

		public override void Perform()
		{
		}
		
		public override void Exit()
		{
			CharacterController.enabled = true;
			InputControl.EnableMovementInput();
			InputControl.EnableButtonsInput();
			base.Exit();
		}
	}
}