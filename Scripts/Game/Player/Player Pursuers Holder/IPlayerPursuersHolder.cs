using FAS.Actors.Emenies;

namespace FAS.Players
{
	public interface IPlayerPursuersHolder
	{
		public void TryRemove(IPursuiter pursuiter);
		
		public void TryAdd(IPursuiter pursuiter);
	}
}