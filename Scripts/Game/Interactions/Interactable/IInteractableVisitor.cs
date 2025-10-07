using FAS.Transitions;
using FAS.Actors.Companion;
using FAS.Items;

namespace FAS
{
	public interface IInteractableVisitor
	{
		public void Apply(LootContainer lootContainer);
		
		public void Apply(ITransitionZone transition);
		
		public void Apply(LootHolder lootHolder);
		
		public void Apply(Companion companion);
		
		public void Apply(DeadBody deadBody);
		
		public void Apply(Flies flies);
		
		public void Apply(Bus bus);
	}
}