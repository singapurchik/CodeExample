namespace FAS.Sounds
{
	public interface ISoundEffectsPlayer
	{
		public void PlayOneShot(SoundEvent soundEvent, bool enqueueClip = false, bool isQueueClosed = false);

		public void Play(SoundEvent soundEvent, bool isLooped = false);

		public void TryStop(SoundEvent soundEvent);

		public void ClearSoundsQueue();
		
		public void TryStop();
	}
}