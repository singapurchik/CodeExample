using UnityEngine;
using Zenject;

namespace FAS.Sounds
{
	public class MusicChanger : MonoBehaviour, IMusicChanger
	{
		[SerializeField] private SoundEvent _corridorBackground;
		[SerializeField] private SoundEvent _streetBackground;
		[SerializeField] private SoundEvent _pursuit;
		
		[Inject] private Music _musicChanger;
				
		private const float DEFAULT_FADE_OUT_DURATION = 2f;
		private const float DEFAULT_FADE_IN_DURATION = 0.25f;
		
		private void Start() => _musicChanger.Play(_streetBackground, true);

		public void PlayCorridorBackgroundMusic(bool isSmoothed = false)
		{
			if (isSmoothed)
				_musicChanger.PlaySmooth(_corridorBackground,
					DEFAULT_FADE_OUT_DURATION, 0, 0, true);
			else
				_musicChanger.Play(_corridorBackground, true);
		}

		public void PlayStreetBackgroundMusic(bool isSmoothed = false)
		{
			if (isSmoothed)
				_musicChanger.PlaySmooth(_streetBackground, 
					DEFAULT_FADE_OUT_DURATION, 0, 0, true);
			else
				_musicChanger.Play(_streetBackground, true);
		}

		public void PlayPursuitMusic() => _musicChanger.Play(_pursuit, true);
		
		public void StopMusic() => _musicChanger.TryStop();

		public void Unmute() => _musicChanger.Unmute();

		public void Mute() => _musicChanger.Mute();
	}
}