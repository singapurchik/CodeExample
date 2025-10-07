using FAS.Players.Animations;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerReactionEffects : MonoBehaviour
	{
		[Inject] private PlayerVisualEffects _visualEffects;
		[Inject] private IPlayerCostumeProxy _costumeProxy;
		[Inject] private PlayerSoundEffects _soundEffects;
		[Inject] private PlayerCameraShaker _cameraShaker;
		[Inject] private PlayerAnimator _animator;
		[Inject] private IReadOnlyHealth _health;

		private void OnEnable()
		{
			_health.OnTakeDamage += PlayDamageFromHit;
		}

		private void OnDisable()
		{
			_health.OnTakeDamage -= PlayDamageFromHit;
		}

		public void PlayDamageFromHit()
		{
			_visualEffects.PlayTakeDamageBloodEffect();
			_soundEffects.PlayTakeDamageVoice();
			_cameraShaker.PlayStabStomach();
			_soundEffects.PlayStabStomach();
			_animator.PlayTakeDamageAnim();
		}

		public void PlayDamageFromFlies()
		{
			_soundEffects.PlayTakeDamageVoice();
			_visualEffects.PlayEaseBloodFrameCameraEffect();
			_cameraShaker.PlayStabStomach();
			_animator.PlayTakeDamageAnim();
		}

		public void PlayDamageFromInjection()
		{
			_soundEffects.ClearSoundsQueue();
			_soundEffects.PlayInjection();
			_soundEffects.PlayTakeDamageVoice();
			_visualEffects.PlayEaseBloodFrameCameraEffect();
			_cameraShaker.PlayStabStomach();
		}

		public void PlayHealFromInjection()
		{
			_soundEffects.PlayInjection(true);
			_soundEffects.PlayerTakeHealVoice(true, true);
			_visualEffects.PlayHealEffect();
		}
	}
}