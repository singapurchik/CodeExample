namespace FAS
{
	public interface IReadOnlyAnimatorLayer
	{
		public int CurrentAnimHash { get; }

		public float CurrentAnimNTime { get; }
		
		public bool IsInTransition { get; }
		public bool IsDisabled { get; }
		public bool IsEnabled { get; }
		public bool IsActive { get; }
		public float Weight { get; }
	}
}