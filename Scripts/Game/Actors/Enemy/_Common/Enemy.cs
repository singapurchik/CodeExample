using UnityEngine;
using Zenject;

namespace FAS.Actors.Emenies
{
	public class Enemy : MonoBehaviour, IPursuiter
	{
		[SerializeField] private ZoneType _zoneType;
		
		[Inject] private EnemyAnimEventsReceiver _animEvents;
		[Inject] private EnemyStateMachine _stateMachine;
		[Inject] private EnemySoundEffects _soundEffects;
		[Inject] private HumanoidBonesHolder _boneHolder;
		[Inject] private TargetDetector _targetDetector;
		[Inject] private EnemyRenderer _renderer;

		public Vector3 AimingPosition => _boneHolder.Head.position;
		public Vector3 Position => transform.position;

		public ZoneType ZoneType => _zoneType;
		
		private void OnEnable()
		{
			_animEvents.OnFootstep += _soundEffects.PlayFootstepSound;
			_targetDetector.OnTargetDetected += OnTargetDetected;
			_targetDetector.OnTargetMissing += OnTargetMissing;
		}

		private void OnDisable()
		{
			_animEvents.OnFootstep -= _soundEffects.PlayFootstepSound;
			_targetDetector.OnTargetDetected -= OnTargetDetected;
			_targetDetector.OnTargetMissing -= OnTargetMissing;
		}

		private void Start() => _stateMachine.Initialize();

		public void PredictHealth(float damage)
		{
			
		}

		public void Deselect()
		{
			_renderer.DisableOutline();
		}

		public void Select()
		{
			_renderer.EnableOutline();
		}

		private void OnTargetMissing() => _targetDetector.CurrentTarget.PursuersHolder.TryRemove(this);
		
		private void OnTargetDetected() => _targetDetector.CurrentTarget.PursuersHolder.TryAdd(this);
	}
}