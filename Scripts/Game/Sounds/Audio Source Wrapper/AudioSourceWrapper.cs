using FAS.Sounds.AudioMixerParametrs;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using DG.Tweening;
using System;

namespace FAS.Sounds
{
	[RequireComponent(typeof(AudioSource))]
	public abstract class AudioSourceWrapper : MonoBehaviour, IPausable
	{
		[SerializeField] private AudioMixer _mixer;
		[SerializeField] private AudioMixerParameter[] _mixerParameters = Array.Empty<AudioMixerParameter>();

		private readonly ResettableQueue<SoundEvent> _soundsQueue = new ();
		private AudioSource _audioSource;
		private Tween _currentFadeTween;

		protected bool IsMuted { get; private set; }

		private float _nextTimePlayClipFromQueue;
		protected float DefaultVolume;

		protected abstract string MixerParameterPrefix { get; }

		private string VolumeParameterName => $"{MixerParameterPrefix} {VOLUME}";

		private const float MIN_VOLUME_DB = -20f;

		private const string VOLUME = "Volume";

		protected virtual void Awake()
		{
			_audioSource = GetComponent<AudioSource>();

			if (_mixer.GetFloat(VolumeParameterName, out var volume))
				DefaultVolume = volume;
		}

		private void OnDisable()
		{
			_currentFadeTween?.Kill();
		}
		
		protected float GetMixerVolume() => _mixer.GetFloat(VolumeParameterName, out var volume) ? volume : 0f;
		
		protected void SetMixerVolume(float volume) => _mixer.SetFloat(VolumeParameterName, volume);

		public void Mute()
		{
			if (!IsMuted)
			{
				IsMuted = true;
				_audioSource.mute = IsMuted;
			}
		}

		public void Unmute()
		{
			if (IsMuted)
			{
				IsMuted = false;
				_audioSource.mute = IsMuted;
			}
		}
		
		public void PlayOneShot(SoundEvent soundEvent, bool enqueueClip = false, bool isQueueClosed = false)
		{
			if (enqueueClip)
			{
				_soundsQueue.Enqueue(soundEvent, isQueueClosed);

				if (isQueueClosed)
					_nextTimePlayClipFromQueue = 0f;
			}
			else
			{
				ApplyMixerParameters(soundEvent.MixerParameters);
				_audioSource.PlayOneShot(soundEvent.GetClip(), soundEvent.Volume);
			}
		}

		public void TryPlay()
		{
			if (_audioSource.clip != null)
			{
				_currentFadeTween?.Kill();
				_audioSource.Play();
			}
		}

		public void TryPause()
		{
			if (_audioSource.clip != null)
				_audioSource.Pause();
		}

		public virtual void Play(SoundEvent soundEvent, bool isLooped = false)
		{
			_currentFadeTween?.Kill();
			ApplyMixerParameters(soundEvent.MixerParameters);
			TryStop();
			SetMixerVolume(soundEvent.VolumeDB);
			_audioSource.clip = soundEvent.GetClip();
			_audioSource.loop = isLooped;
			_audioSource.Play();
		}
		
		public void TryStop(SoundEvent soundEvent)
		{
			if (_audioSource.clip != null && _audioSource.isPlaying && _audioSource.clip == soundEvent.CurrentClip)
				Stop();
		}

		public void TryStop()
		{
			if (_audioSource.clip != null && _audioSource.isPlaying)
				Stop();
		}

		private void Stop()
		{
			_currentFadeTween?.Kill();
			_audioSource.Stop();
			SetMixerVolume(0);
			_audioSource.clip = null;
		}

		protected void ApplyMixerParameter(AudioMixerParameterConfig config)
		{
			foreach (var mixerParameter in _mixerParameters)
			{
				if (mixerParameter == config.Type)
				{
					config.Apply(_mixer, MixerParameterPrefix);
					break;
				}
			}
		}

		protected void ApplyMixerParameters(List<AudioMixerParameterConfig> configs)
		{
			foreach (var mixerParameter in _mixerParameters)
			{
				foreach (var config in configs)
				{
					if (mixerParameter == config.Type)
					{
						config.Apply(_mixer, MixerParameterPrefix);
						break;
					}
				}
			}
		}
		
		public void PlaySmooth(SoundEvent soundEvent, float fadeOutDuration,
			float fadeInDuration, float fadeInDelay = 0f, bool isLooped = false, bool applyMixerParamsOnSameClip = true)
		{
			_currentFadeTween?.Kill();

			var newClip = soundEvent.GetClip();
			float currentVolume = GetMixerVolume();

			bool hasClip = _audioSource.clip != null;
			bool isSameClip = hasClip && _audioSource.clip == newClip;

			if (hasClip)
			{
				if (isSameClip)
				{
					if (applyMixerParamsOnSameClip)
						ApplyMixerParameters(soundEvent.MixerParameters);

					if (!_audioSource.isPlaying)
						_audioSource.Play();

					if (_audioSource.loop != isLooped)
						_audioSource.loop = isLooped;

					if (Mathf.Abs(currentVolume - soundEvent.VolumeDB) > 0.1f)
						Fade(currentVolume, soundEvent.VolumeDB, fadeInDuration, fadeInDelay);
				}
				else
				{
					if (fadeOutDuration <= 0f)
					{
						SetMixerVolume(MIN_VOLUME_DB);
						Play(soundEvent, isLooped);
						Fade(MIN_VOLUME_DB, soundEvent.VolumeDB, fadeInDuration, fadeInDelay);
					}
					else
					{
						_currentFadeTween = DOVirtual
							.Float(currentVolume, MIN_VOLUME_DB, fadeOutDuration, SetMixerVolume)
							.SetEase(Ease.Linear)
							.OnComplete(() =>
							{
								Play(soundEvent, isLooped);
								SetMixerVolume(MIN_VOLUME_DB);
								Fade(MIN_VOLUME_DB, soundEvent.VolumeDB, fadeInDuration, fadeInDelay);
							});
					}
				}
			}
			else
			{
				Play(soundEvent, isLooped);
				SetMixerVolume(MIN_VOLUME_DB);
				Fade(MIN_VOLUME_DB, soundEvent.VolumeDB, fadeInDuration, fadeInDelay);
			}
		}

		private void Fade(float from, float to, float duration, float delay = 0f, Action onComplete = null)
		{
			_currentFadeTween?.Kill();

			if (duration <= 0f)
			{
				SetMixerVolume(to);
				onComplete?.Invoke();
			}
			else
			{
				_currentFadeTween = DOVirtual.Float(from, to, duration, SetMixerVolume)
					.SetEase(Ease.Linear)
					.SetDelay(delay)
					.OnComplete(() => onComplete?.Invoke());	
			}
		}

		
		public void ClearSoundsQueue() => _soundsQueue.Clear();

		private void Update()
		{
			if (_soundsQueue.Count > 0
			    && Time.timeSinceLevelLoad > _nextTimePlayClipFromQueue
			    && _soundsQueue.TryDequeue(out var soundEvent))
			{
				PlayOneShot(soundEvent);
				_nextTimePlayClipFromQueue = Time.timeSinceLevelLoad + soundEvent.CurrentClip.length;
			}
		}

		public virtual void Pause() => ClearSoundsQueue();

		public abstract void Play();
	}
}