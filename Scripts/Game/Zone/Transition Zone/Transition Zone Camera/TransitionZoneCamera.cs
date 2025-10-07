using UnityEngine.Events;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using FAS.Sounds;
using Zenject;
using System;

namespace FAS.Transitions
{
	[Serializable]
	public class TransitionZoneCamera : ITransitionZoneCamera
	{
		[SerializeField] private CinemachineVirtualCamera _camera;
		[SerializeField] private float _animOutDuration = 0.5f;
		[SerializeField] private Ease _animOutEase = Ease.Linear;
		[SerializeField] private float _animInDuration = 0.5f;
		[SerializeField] private Ease _animInEase = Ease.OutExpo;
		[Range (0, 1)] [SerializeField] private float _cameraBlendWeightForEnterAnim = 0.5f;
		[SerializeField] private SoundEvent _enterStartedSound;
		[SerializeField] private SoundEvent _exitStartedSound;
		[SerializeField] private UnityEvent _onExitFinished;
		[SerializeField] private UnityEvent _onEnterFinished;
		[SerializeField] private UnityEvent _onEnterStarted;
		[SerializeField] private UnityEvent _onExitStarted;
		
		[Inject] private ISoundEffectsPlayer _soundEffects;
		[Inject] private ICinemachineBrainInfo _brainInfo;
		
		private CinemachineTrackedDolly _dolly;
		private Tween _currentCameraTween;

		public Coroutine CurrentRoutine { get; private set; }
		
		public UnityEvent OnExitFinished => _onExitFinished;
		public UnityEvent OnEnterFinished => _onEnterFinished;
		public UnityEvent OnEnterStarted => _onEnterStarted;
		public UnityEvent OnExitStarted => _onExitStarted;

		public void Initialize()
		{
			_dolly = _camera.GetCinemachineComponent<CinemachineTrackedDolly>();
		}

		public void Disable() => _camera.gameObject.SetActive(false);

		public IEnumerator Enter()
		{
			ReadyToMove(0, _enterStartedSound, OnEnterStarted);
			yield return new WaitUntil(() =>
				_brainInfo.IsBlending(out var nTime, out var weight)
				&& weight > _cameraBlendWeightForEnterAnim);
			Move(1, _animInDuration, _animInEase, OnEnterFinished);
		}

		public void Exit()
		{
			ReadyToMove(1, _exitStartedSound, OnExitStarted);
			Move(0, _animOutDuration, _animOutEase, OnExitFinished);
		}

		private void ReadyToMove(float pathFrom, SoundEvent moveSound, UnityEvent startAction)
		{
			startAction?.Invoke();
			_soundEffects.PlayOneShot(moveSound);
			_camera.gameObject.SetActive(true);
			_currentCameraTween?.Kill();
			_dolly.m_PathPosition = pathFrom;
		}

		private void Move(float pathTo, float duration, Ease easeType, UnityEvent completeAction)
		{
			_currentCameraTween = DOVirtual.Float(_dolly.m_PathPosition, pathTo, duration,
					value => _dolly.m_PathPosition = value)
				.SetEase(easeType)
				.OnComplete(() => { completeAction?.Invoke(); });
		}
	}
}