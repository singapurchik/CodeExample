namespace FAS.UI
{
	public interface IReadOnlyUIScreen : IReadOnlyUIScreenEvents
	{
		public float ShowFadeDuration { get; }
		public float HideFadeDuration { get; }
		
		public bool IsProcessShow { get; }
		public bool IsProcessHide { get; }
		public bool IsShown { get; }
	}
}