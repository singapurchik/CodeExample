namespace FAS.Actors.Emenies.Animations
{
	public interface IReadOnlyUpperBodyAdditiveLayer : IReadOnlyAnimatorLayer
	{
		public int IdleAnimHash { get; }
		public int WalkAnimHash { get; }
		public int RunAnimHash { get; }
	}
}