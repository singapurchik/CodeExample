namespace FAS.Fatality
{
	public interface IFatalityTarget : IReadOnlyFatalityTarget
	{
		public bool IsFatalityCompleted { get; }
		
		public void PrepareFatality(FatalityData data);
		
		public void PerformFatality();
		
		public void FinishFatality();
	}
}