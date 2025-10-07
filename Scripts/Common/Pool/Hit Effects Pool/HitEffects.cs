using UnityEngine;
using System;

namespace FAS
{
	public class HitEffects : MonoBehaviour
	{
		[SerializeField] private ParticleSystem _fatalityTornadoKickEffect;
		[SerializeField] private ParticleSystem _punchHit;

		private ParticleSystem _currentEffect;
		
		private bool _isActive;
		
		public event Action<HitEffects> OnComplete;

		public void Initialize() => gameObject.SetActive(false);

		public void PlayFatalityTornadoKickHitEffect(Vector3 position) => Play(position, _fatalityTornadoKickEffect);
		
		public void PlayPunchHitEffect(Vector3 position) => Play(position, _punchHit);

		private void Play(Vector3 position, ParticleSystem effect)
		{
			_isActive = true;
			transform.position = position;
			_currentEffect = effect;
			_currentEffect.Play();
		}

		public void Stop()
		{
			OnComplete?.Invoke(this);
			_isActive = false;
		}

		private void Update()
		{
			if (_isActive && !_currentEffect.isPlaying && !_currentEffect.IsAlive(true))
				Stop();
		}
	}
}