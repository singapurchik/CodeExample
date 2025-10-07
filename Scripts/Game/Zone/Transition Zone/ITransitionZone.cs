namespace FAS.Transitions
{
	public interface ITransitionZone
	{
		public ITransitionZoneCamera Camera { get; }
		public TransitionZoneData Data { get; }
	}
}