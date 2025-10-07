namespace FAS.Players.Animations
{
	public interface IReadOnlyFullBodyReactionLayer : IReadOnlyAnimatorLayer
	{
		public int KnockedDownAnimHash { get; }
		public int FlyingBackAnimHash { get; }
		public int EdgeSlipAnimHash { get; }
		public int StandUpAnimHash { get; }
	}
}