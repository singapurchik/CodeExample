namespace FAS.Actors.Companion
{
	public interface ICompanion : ICharacterInfo
	{
		public ICompanionOwner Owner { get; }
		
		public bool IsInitialized { get; }
	}
}