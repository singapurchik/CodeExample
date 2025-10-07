using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using FAS.Players;
using FAS.Sounds;
using Zenject;
using System;
using FAS.UI;

namespace FAS.Apartments.Inside
{
	public class Jumpscare : MonoBehaviour
	{
		[SerializeField] private GameObject _screamer;
		[SerializeField] private Animator _animator;
		[SerializeField] private float _startDelay;
		[SerializeField] private float _screamVerticalPosition;
		[SerializeField] private float _moveTime = 1f;
		[SerializeField] private Ease _moveEase;
		[SerializeField] private float _finishDelay = 1f;
		[SerializeField] private SoundEvent _jumpscareSound;
		[SerializeField] private CinemachineImpulseSource _jumpscareCameraImpulse;

		[Inject] private ISoundEffectsPlayer _soundEffects;
		[Inject] private IUIFadingFrame _uiFadingFrame;
		[Inject] private ICameraShaker _cameraShaker;
		
		private Quaternion _originalRotation;
		private Transform _originalParent;
		private Vector3 _originalPosition;
		private Vector3 _originalScale;
		
		private Coroutine _currentFinishRoutine;
		private Tween _currentMoveTween;

		public event Action OnFinishScreamingToWindow;

		private void Awake()
		{
			_originalPosition = transform.position;
			_originalRotation = transform.rotation;
			_originalScale = transform.localScale;
			_originalParent = transform.parent;
			_screamer.SetActive(false);
		}

		public void PlayScreamingToWindow()
		{
			if (_currentFinishRoutine != null)
			{
				StopCoroutine(_currentFinishRoutine);
				_screamer.SetActive(false);
			}

			_currentMoveTween?.Kill();
			transform.SetParent(_originalParent);
			transform.position = _originalPosition;
			transform.rotation = _originalRotation;
			transform.localScale = _originalScale;
			
			transform.DOLocalMoveY(_screamVerticalPosition, _moveTime)
				.SetEase(_moveEase)
				.SetDelay(_startDelay)
				.OnComplete(() => _currentFinishRoutine = StartCoroutine(FinishScreamingToWindow()));

			_soundEffects.PlayOneShot(_jumpscareSound);
			StartCoroutine(PlayUIEffects());
			ReadyToPlay();
			Play();
		}
		
		private IEnumerator PlayUIEffects()
		{
			yield return new WaitForSeconds(_finishDelay / 2);
			_cameraShaker.PlayImpulse(_jumpscareCameraImpulse);
			_uiFadingFrame.PlayJumpscare();
		}

		private IEnumerator FinishScreamingToWindow()
		{
			yield return new WaitForSeconds(_finishDelay);
			OnFinishScreamingToWindow?.Invoke();
			_screamer.SetActive(false);
		}

		public void ReadyToPlay()
		{
			_animator.Rebind();
			_animator.speed = 0;
		}

		public void Play()
		{
			if (_currentFinishRoutine != null)
				StopCoroutine(_currentFinishRoutine);

			_screamer.SetActive(true);
			_animator.speed = 1;
		}

		public void SetParent(Transform parent, bool isEnabled)
		{
			transform.SetParent(parent);
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			_screamer.SetActive(isEnabled);
		}
	}	
}