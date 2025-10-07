namespace FAS.Sounds
{
	public class Music : AudioSourceWrapper
	{
		protected override string MixerParameterPrefix => "Music";

		private bool _isMutedBeforePause;
		
		public override void Play()
		{
			if (!_isMutedBeforePause)
				Unmute();
		}

		public override void Pause()
		{
			_isMutedBeforePause = IsMuted;
			base.Pause();
			Mute();
		}
	}
}