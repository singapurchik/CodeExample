namespace FAS.Players
{
	public interface IPlayerStateReturner : IStateReturner
	{
		public void TryReturnLastControlledState();
	}
}