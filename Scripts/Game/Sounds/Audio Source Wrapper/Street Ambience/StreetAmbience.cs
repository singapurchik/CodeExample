using FAS.Sounds.AudioMixerParametrs;
using UnityEngine;

namespace FAS.Sounds
{
	public class StreetAmbience : AudioSourceWrapper, IStreetAmbience
	{
		[SerializeField] private SoundEvent _rainAmbience;
		[SerializeField] private LowpassParameterConfig _outdoorLowpass;
		[SerializeField] private LowpassParameterConfig _throughWindowLowpass;
		[SerializeField] private LowpassParameterConfig _indoorLowpass;
		
		protected override string MixerParameterPrefix => "Street Ambience";
		
		private float _volumeBeforePause;

		private bool _isPlaying;
		
		private const float PAUSE_VOLUME_STEP = -20f;

		private void Start()
		{
			Play(_rainAmbience, true);
			_isPlaying = true;
		}

		public void PlayThroughWindow() => ApplyMixerParameter(_throughWindowLowpass);

		public void PlayOutdoor() => ApplyMixerParameter(_outdoorLowpass);

		public void PlayIndoor() => ApplyMixerParameter(_indoorLowpass);

		public override void Pause()
		{
			base.Pause();
			_volumeBeforePause = GetMixerVolume();
			SetMixerVolume(_volumeBeforePause + PAUSE_VOLUME_STEP);
		}

		public override void Play() => SetMixerVolume(_volumeBeforePause);
	}
}