namespace FAS.Sounds
{
	public interface IVoicePlayer
	{
		public void Play(SoundEvent soundEvent, bool isLooped = false);

		public void TryStop();
	}
}