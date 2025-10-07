namespace FAS.Players
{
	public interface IPlayerInfo : ICharacterInfo
	{
		public bool IsCrouched { get; }
	}
}