using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace FAS.Environment
{
	public class LampFlicker : MonoBehaviour
	{
		[Header("Target")]
		[SerializeField] private Renderer _targetRenderer;

		[Header("Playback")]
		[SerializeField, Tooltip("Start automatically on OnEnable")]
		private bool _autoPlay = true;

		[Header("Emission (HDR)")]
		[SerializeField, ColorUsage(true, true)]
		private Color _baseEmissionColor = Color.white;

		[SerializeField, Min(0f)] private float _normalIntensity = 2f;
		[SerializeField, Min(0f)] private float _dimIntensity = 0.1f;

		[Header("Pattern Weights")]
		[SerializeField, Range(0f, 1f)] private float _wIdle = 0.45f;

		[SerializeField, Range(0f, 1f)] private float _wFade = 0.25f;
		[SerializeField, Range(0f, 1f)] private float _wBurst = 0.25f;
		[SerializeField, Range(0f, 1f)] private float _wBlackout = 0.05f;

		[Header("Idle")]
		[SerializeField] private Vector2 _idleHold = new(1f, 4f);

		[Header("Fade")]
		[SerializeField] private Vector2 _fadeDownTime = new(0.08f, 0.25f);

		[SerializeField] private Vector2 _fadeUpTime = new(0.08f, 0.25f);
		[SerializeField] private int _fadeRepeatsMin = 1;
		[SerializeField] private int _fadeRepeatsMax = 2;

		[Header("Burst")]
		[SerializeField] private Vector2 _burstInterval = new(0.03f, 0.12f);

		[SerializeField] private int _burstFlashesMin = 3;
		[SerializeField] private int _burstFlashesMax = 8;

		[Header("Blackout")]
		[SerializeField] private Vector2 _blackoutHold = new(0.3f, 1.2f);

		[Header("Randomness")]
		[SerializeField, Tooltip("Deterministic sequence when > 0")]
		private int _randomSeed = 0;

		private MaterialPropertyBlock _materialPropertyBlock;
		private System.Random _rng;
		private Coroutine _runner;
		private Tween _tween;

		private float _intensity;

		private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");

		private enum Episode
		{
			Idle,
			Fade,
			Burst,
			Blackout
		}

		private void Reset()
		{
			_targetRenderer = GetComponent<Renderer>();
		}

		private void Awake()
		{
			_materialPropertyBlock = new MaterialPropertyBlock();
			_rng = _randomSeed == 0 ? new System.Random() : new System.Random(_randomSeed);
			SetIntensityImmediate(_normalIntensity);
		}

		private void OnEnable()
		{
			if (_autoPlay)
				Play();
		}

		private void OnDisable()
		{
			Stop();
		}

		public void Play()
		{
			Stop();
			_runner = StartCoroutine(RunPattern());
		}

		public void Stop()
		{
			if (_runner != null)
			{
				StopCoroutine(_runner);
				_runner = null;
			}

			if (_tween != null)
			{
				_tween.Kill();
				_tween = null;
			}

			SetIntensityImmediate(_normalIntensity);
		}

		private IEnumerator RunPattern()
		{
			while (true)
			{
				switch (PickEpisode())
				{
					case Episode.Idle: yield return Idle(); break;
					case Episode.Fade: yield return Fade(); break;
					case Episode.Burst: yield return Burst(); break;
					default: yield return Blackout(); break;
				}
			}
		}

		private Episode PickEpisode()
		{
			var sum = _wIdle + _wFade + _wBurst + _wBlackout;
			var r = NextFloat() * Mathf.Max(sum, 0.0001f);
			if ((r -= _wIdle) <= 0f) return Episode.Idle;
			if ((r -= _wFade) <= 0f) return Episode.Fade;
			if ((r -= _wBurst) <= 0f) return Episode.Burst;
			return Episode.Blackout;
		}

		private IEnumerator Idle()
		{
			yield return Wait(RandRange(_idleHold));
		}

		private IEnumerator Fade()
		{
			var repeats = _rng.Next(_fadeRepeatsMin, _fadeRepeatsMax + 1);
			for (int i = 0; i < repeats; i++)
			{
				yield return TweenIntensity(_intensity, _dimIntensity, RandRange(_fadeDownTime), Ease.InOutSine);
				yield return TweenIntensity(_dimIntensity, _normalIntensity, RandRange(_fadeUpTime), Ease.InOutSine);
			}
		}

		private IEnumerator Burst()
		{
			var flashes = _rng.Next(_burstFlashesMin, _burstFlashesMax + 1);
			for (int i = 0; i < flashes; i++)
			{
				SetIntensityImmediate(_normalIntensity * 1.2f);
				yield return Wait(RandRange(_burstInterval) * 0.4f);
				SetIntensityImmediate(_dimIntensity);
				yield return Wait(RandRange(_burstInterval));
			}

			yield return TweenIntensity(_intensity, _normalIntensity, 0.08f, Ease.OutSine);
		}

		private IEnumerator Blackout()
		{
			SetIntensityImmediate(0f);
			yield return Wait(RandRange(_blackoutHold));
			yield return TweenIntensity(_intensity, _normalIntensity, 0.18f, Ease.OutSine);
		}

		private IEnumerator TweenIntensity(float from, float to, float duration, Ease ease)
		{
			_tween?.Kill();
			float v = from;
			_tween = DOTween.To(() => v, x =>
				{
					v = x;
					SetIntensityImmediate(x);
				}, to, duration)
				.SetEase(ease)
				.SetUpdate(UpdateType.Normal, true);
			yield return _tween.WaitForCompletion();
		}

		private void SetIntensityImmediate(float value)
		{
			_intensity = Mathf.Max(0f, value);
			var final = _baseEmissionColor * _intensity;
			_targetRenderer.GetPropertyBlock(_materialPropertyBlock);
			_materialPropertyBlock.SetColor(EmissionColorID, final);
			_targetRenderer.SetPropertyBlock(_materialPropertyBlock);
		}

		private static WaitForSecondsRealtime Wait(float t) => new(Mathf.Max(0f, t));
		
		private float RandRange(Vector2 r) => Mathf.Lerp(r.x, r.y, NextFloat());
		
		private float NextFloat() => (float)_rng.NextDouble();
	}
}