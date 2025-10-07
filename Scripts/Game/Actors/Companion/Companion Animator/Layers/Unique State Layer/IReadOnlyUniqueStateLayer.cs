namespace FAS.Actors.Companion.Animations
{
	public interface IReadOnlyUniqueStateLayer : IReadOnlyAnimatorLayer
	{
		public int StandUpFromBedAnimHash { get; }
		public int ChillingOnBedAnimHash { get; }
	}
}