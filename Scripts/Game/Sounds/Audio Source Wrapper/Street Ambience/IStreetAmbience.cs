namespace FAS.Sounds
{
	public interface IStreetAmbience : IMuteable
	{
		public void PlayThroughWindow();
		
		public void PlayOutdoor();
		
		public void PlayIndoor();
	}
}