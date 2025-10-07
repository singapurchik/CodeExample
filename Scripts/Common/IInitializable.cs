namespace FAS
{
	public interface IInitializable
	{
		public bool IsInitialized { get; }
		
		public void PrepareToInitialize();
		public void Initialize();
	}
}