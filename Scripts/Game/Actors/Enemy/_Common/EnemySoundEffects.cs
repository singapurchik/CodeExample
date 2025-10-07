using System.Collections;
using UnityEngine;
using FAS.Sounds;
using Zenject;

namespace FAS.Actors.Emenies
{
	public class EnemySoundEffects : MonoBehaviour, IPausable
	{
		[SerializeField] private SoundEvent _faceCameraFlySound;
		[SerializeField] private float _faceCameraFlyEnableDelay = 0.3f;
		[SerializeField] private SoundEvent _footstepSound;
		[SerializeField] private SoundEvent _loseSound;
		[SerializeField] private SoundEvent _meleeAttackSound;

		[Inject] private ISoundEffectsPlayer _soundEffectsPlayer;
		[Inject] private AudioSource _audioSource;

		public void PlayLoseSound(float delay) => StartCoroutine(Play2DEffectWithDelay(_loseSound, delay));

		public void PlayFootstepSound() => _audioSource.PlayOneShot(_footstepSound.GetClip(), _footstepSound.Volume);
		
		public void PlayFaceCameraFly()
			=> StartCoroutine(Play2DEffectWithDelay(_faceCameraFlySound, _faceCameraFlyEnableDelay));

		public void PlayMeleeAttackSound() => _soundEffectsPlayer.PlayOneShot(_meleeAttackSound);

		private IEnumerator Play2DEffectWithDelay(SoundEvent soundEvent, float delay)
		{
			yield return new WaitForSeconds(delay);
			_soundEffectsPlayer.PlayOneShot(soundEvent);
		}

		public void Pause() => _audioSource.volume = 0f;
		
		public void Play() => _audioSource.volume = 1f;
	}
}