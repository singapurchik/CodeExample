namespace FAS.Sounds
{
	public interface IMusicChanger : IMuteable
	{
		public void PlayCorridorBackgroundMusic(bool isSmoothed = false);

		public void PlayStreetBackgroundMusic(bool isSmoothed = false);
		
		public void PlayPursuitMusic();
		
		public void StopMusic();
	}
}