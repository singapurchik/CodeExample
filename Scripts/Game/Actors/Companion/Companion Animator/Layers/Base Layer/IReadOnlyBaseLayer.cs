namespace FAS.Actors.Companion.Animations
{
	public interface IReadOnlyBaseLayer : IReadOnlyAnimatorLayer
	{
		public int KnockedBackAnimHash { get; }
		
		public bool IsIdleAnimPlaying { get; }
	}
}