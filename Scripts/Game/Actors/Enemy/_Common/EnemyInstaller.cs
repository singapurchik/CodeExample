using FAS.Actors.Emenies.Animations;
using System.Collections.Generic;
using UnityEngine.AI;
using Cinemachine;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Actors.Emenies
{
	public class EnemyInstaller : MonoInstaller
	{
		[Tab("Common")]
		[SerializeField] private EnemyAnimEventsReceiver _animEvents;
		[SerializeField] private CinemachineVirtualCamera _faceCamera;
		[SerializeField] private HumanoidBonesHolder _bonesHolder;
		[SerializeField] private EnemyPatrolPoints _patrolPoints;
		[SerializeField] private EnemySoundEffects _soundEffects;
		[SerializeField] private TargetDetector _targetDetector;
		[SerializeField] private Animator _animatorController;
		[SerializeField] private NavMeshAgent _navMeshAgent;
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private EnemyAnimator _animator;
		[SerializeField] private EnemyRenderer _renderer;
		[SerializeField] private EnemyRotator _rotator;
		[SerializeField] private Pausable _pausable;
		[SerializeField] private EnemyMover _mover;
		[SerializeField] private List<Renderer> _renderers = new ();
		[EndTab]
		[Tab("State Machine")]
		[SerializeField] private FatalityExecute _fatalityExecuteState;
		[SerializeField] private EnemyStateMachine _stateMachine;
		[SerializeField] private ShowingFace _showingFaceState;
		[SerializeField] private Patrol _patrolState;
		[SerializeField] private Attack _attackState;
		[SerializeField] private Chase _chaseState;
		[SerializeField] private Idle _idleState;
		[EndTab]

		public override void InstallBindings()
		{
			BindStateMachine();
			InjectPausables();
			BindCommons();
		}

		private void BindStateMachine()
		{
			Container.Bind<IEnemyStateMachineInfo>().FromInstance(_stateMachine).AsSingle();

			Container.BindInstance(_audioSource).WhenInjectedInto<EnemySoundEffects>();
			Container.BindInstance(_faceCamera).WhenInjectedInto<ShowingFace>();
			Container.BindInstance(_stateMachine).WhenInjectedInto<Enemy>();
			Container.BindInstance(_patrolPoints).WhenInjectedInto<Patrol>();

			BindInstanceToStateMachine(_fatalityExecuteState);
			BindInstanceToStateMachine(_showingFaceState);
			BindInstanceToStateMachine(_patrolState);
			BindInstanceToStateMachine(_attackState);
			BindInstanceToStateMachine(_chaseState);
			BindInstanceToStateMachine(_idleState);
		}
		
		private void BindInstanceToStateMachine<T>(T instance)
			=> Container.BindInstance(instance).WhenInjectedInto<EnemyStateMachine>();
		
		private void InjectPausables()
		{
			var list = new List<IPausable>(10)
			{
				_animator,
				_soundEffects
			};
			Container.BindInstance(list).WhenInjectedIntoInstance(_pausable);
			Container.Bind<IPausableInfo>().FromInstance(_pausable).AsSingle();
		}
		
		private void BindCommons()
		{
			CreateAndBindAnimationLayers();
			BindFromInstance();
			BindInstance();
		}

		private void CreateAndBindAnimationLayers()
		{
			var triggersList = new List<ILayerWithTriggers>(10);
			var layersList = new List<Layer>(10);
			
			BindInstanceToDefenderAnimator(new BaseLayer(_animatorController, 0, layersList));
			BindInstanceToDefenderAnimator(new UpperBodyAdditiveLayer(
				_animatorController, 1, layersList, triggersList));
			BindInstanceToDefenderAnimator(new AttackLayer(_animatorController, 2, layersList));
			BindInstanceToDefenderAnimator(new FatalityLayer(_animatorController, 3, layersList));
			
			BindInstanceToDefenderAnimator(_animatorController);
			BindInstanceToDefenderAnimator(triggersList);
			BindInstanceToDefenderAnimator(layersList);
		}

		private void BindInstanceToDefenderAnimator<T>(T instance)
			=> Container.BindInstance(instance).WhenInjectedInto<EnemyAnimator>();
		
		private void BindFromInstance()
		{
			Container.Bind<IReadOnlyList<Renderer>>().FromInstance(_renderers).WhenInjectedIntoInstance(_renderer);
			Container.Bind<ActorAnimator>().FromInstance(_animator).WhenInjectedInto<ActorMover>();
			Container.Bind<IEnemyPatrolPointHolder>().FromInstance(_patrolPoints).AsSingle();
		}

		private void BindInstance()
		{
			Container.BindInstance(_navMeshAgent).WhenInjectedIntoInstance(_rotator);
			Container.BindInstance(_navMeshAgent).WhenInjectedIntoInstance(_mover);
			Container.BindInstance(_targetDetector).AsSingle();
			Container.BindInstance(_soundEffects).AsSingle();
			Container.BindInstance(_bonesHolder).AsSingle();
			Container.BindInstance(_animEvents).AsSingle();
			Container.BindInstance(_animator).AsSingle();
			Container.BindInstance(_renderer).AsSingle();
			Container.BindInstance(_rotator).AsSingle();
			Container.BindInstance(_mover).AsSingle();
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_renderers.Clear();
			_renderers.AddRange(GetComponentsInChildren<Renderer>(true));
			
			_fatalityExecuteState = GetComponentInChildren<FatalityExecute>(true);
			_stateMachine = GetComponentInChildren<EnemyStateMachine>(true);
			_showingFaceState = GetComponentInChildren<ShowingFace>(true);
			_patrolState = GetComponentInChildren<Patrol>(true);
			_attackState = GetComponentInChildren<Attack>(true);
			_chaseState = GetComponentInChildren<Chase>(true);
			_idleState = GetComponentInChildren<Idle>(true);
			
			_animEvents = GetComponentInChildren<EnemyAnimEventsReceiver>(true);
			_faceCamera = GetComponentInChildren<CinemachineVirtualCamera>(true);
			_bonesHolder = GetComponentInChildren<HumanoidBonesHolder>(true);
			_soundEffects = GetComponentInChildren<EnemySoundEffects>(true);
			_patrolPoints = GetComponentInChildren<EnemyPatrolPoints>(true);
			_targetDetector = GetComponentInChildren<TargetDetector>(true);
			_animatorController = GetComponentInChildren<Animator>(true);
			_audioSource = GetComponentInChildren<AudioSource>(true);
			_navMeshAgent = GetComponentInChildren<NavMeshAgent>(true);
			_animator = GetComponentInChildren<EnemyAnimator>(true);
			_renderer = GetComponentInChildren<EnemyRenderer>(true);
			_rotator = GetComponentInChildren<EnemyRotator>(true);
			_pausable = GetComponentInChildren<Pausable>(true);
			_mover = GetComponentInChildren<EnemyMover>(true);
		}
#endif
	}	
}