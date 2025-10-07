using System.Collections.Generic;
using UnityEngine;
using FAS.Sounds;
using Zenject;

namespace FAS.Players
{
	public class PlayerSoundEffects : MonoBehaviour
	{
		[SerializeField] private SoundEvent _knockDownAndStandUp;
		[SerializeField] private SoundEvent _showItemFirstTime;
		[SerializeField] private SoundEvent _knifeOutOfStomach;
		[SerializeField] private SoundEvent _takeDamageVoice;
		[SerializeField] private SoundEvent _takeHealVoice;
		[SerializeField] private SoundEvent _leftLegSlash;
		[SerializeField] private SoundEvent _footstepWalk;
		[SerializeField] private SoundEvent _stabStomach;
		[SerializeField] private SoundEvent _footstepRun;
		[SerializeField] private SoundEvent _heartPulse;
		[SerializeField] private SoundEvent _neckSlash;
		[SerializeField] private SoundEvent _injection;
		[SerializeField] private SoundEvent _addItem;
		[SerializeField] private List<SoundEvent> _jumpscares;
		
		[Inject] private ISoundEffectsPlayer _soundEffects;
		
		public void TryStopHeartPulseSound() => _soundEffects.TryStop(_heartPulse);
		
		public void PlayHeartPulseSound() => _soundEffects.Play(_heartPulse);
		
		public void PlayJumpscare(int index = 0, bool enqueueClip = false, bool isQueueClosed = false)
			=> PlayOneShot(_jumpscares[index]);
		
		public void PlayKnockDownAndStandUp(bool enqueueClip = false, bool isQueueClosed = false)
			=> PlayOneShot(_knockDownAndStandUp, enqueueClip, isQueueClosed);
		
		public void PlayKnifeOutOfStomach(bool enqueueClip = false, bool isQueueClosed = false)
			=> PlayOneShot(_knifeOutOfStomach, enqueueClip, isQueueClosed);
		
		public void PlayShowItemFirstTime(bool enqueueClip = false, bool isQueueClosed = false)
			=> PlayOneShot(_showItemFirstTime, enqueueClip, isQueueClosed);
		
		public void PlayTakeDamageVoice(bool enqueueClip = false, bool isQueueClosed = false)
			=> _soundEffects.PlayOneShot(_takeDamageVoice, enqueueClip, isQueueClosed);
		
		public void PlayerTakeHealVoice(bool enqueueClip = false, bool isQueueClosed = false)
			=> _soundEffects.PlayOneShot(_takeHealVoice, enqueueClip, isQueueClosed);
		
		public void PlayInjection(bool enqueueClip = false, bool isQueueClosed = false)
			=> _soundEffects.PlayOneShot(_injection, enqueueClip, isQueueClosed);
		
		public void PlayFootstepsWalk(bool enqueueClip = false, bool isQueueClosed = false)
			=> PlayOneShot(_footstepWalk, enqueueClip, isQueueClosed);
		
		public void PlayLeftLegSlash(bool enqueueClip = false, bool isQueueClosed = false)
			=> PlayOneShot(_leftLegSlash, enqueueClip,  isQueueClosed);
		
		public void PlayFootstepsRun(bool enqueueClip = false, bool isQueueClosed = false)
			=> PlayOneShot(_footstepRun, enqueueClip,  isQueueClosed);

		public void PlayStabStomach(bool enqueueClip = false, bool isQueueClosed = false)
			=> PlayOneShot(_stabStomach, enqueueClip,  isQueueClosed);
		
		public void PlayNeckSlash(bool enqueueClip = false, bool isQueueClosed = false)
			=> PlayOneShot(_neckSlash, enqueueClip,  isQueueClosed);

		public void PlayAddItem(bool enqueueClip = false, bool isQueueClosed = false)
			=> PlayOneShot(_addItem, enqueueClip,  isQueueClosed);

		public void ClearSoundsQueue() => _soundEffects.ClearSoundsQueue();
		
		private void PlayOneShot(SoundEvent soundEvent, bool enqueueClip = false, bool isQueueClosed = false)
			=> _soundEffects.PlayOneShot(soundEvent, enqueueClip, isQueueClosed);
	}
}