using FAS.Triggers.Corridor;

namespace FAS.Triggers
{
	public interface ITriggerVisitor
	{
		public void Apply(MoveFromExitToStartCorridor moveFromExitToStartCorridor);
	}
}