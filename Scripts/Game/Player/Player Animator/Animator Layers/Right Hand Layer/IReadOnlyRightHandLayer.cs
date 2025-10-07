namespace FAS.Players.Animations
{
	public interface IReadOnlyRightHandLayer : IReadOnlyAnimatorLayer
	{
		public int PickUpShortAnimHash { get; }
		public int PickUpFullAnimHash { get; }
		public int InteractAnimHash { get; }
	}
}