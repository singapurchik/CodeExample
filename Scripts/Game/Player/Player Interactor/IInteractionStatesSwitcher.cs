using FAS.Transitions;

namespace FAS.Players
{
	public interface IInteractionStatesSwitcher : IPlayerStateReturner
	{
		public void SwitchToInteractionWithApartmentWindow(ITransitionZone transition);
		
		public void SwitchToTeleportWithCameraEffectState(ITransitionZone transition);
		
		public void SwitchToInteractionWithLootHolder(LootHolder lootHolder);
		
		public void SwitchToMonologueState(MonologueData monologueData);
		
		public void SwitchToInteractionWithDeadBody(DeadBody deadBody);
		
		public void SwitchToNonInteractionState();
		
		public void SwitchToRotateToInteraction();
		
		public void SwitchToMoveToInteraction();
		
	}
}