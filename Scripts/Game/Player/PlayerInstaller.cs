using System.Collections.Generic;
using FAS.Players.Animations;
using FAS.Players.AnimRig;
using FAS.Players.States;
using FAS.Fatality;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Players
{
	public class PlayerInstaller : MonoInstaller
	{
		[SerializeField] private List<ClothesWetness> _clothesWetnesses = new();
		[SerializeField] private PlayerCharacterController _playerCharacterController;
		[SerializeField] private PlayerInteractableFinder _interactableFinder;
		[SerializeField] private PlayerAnimEventsReceiver _animEventsReceiver;
		[SerializeField] private PuppetMasterHandler _puppetMasterHandler;
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private PlayerReactionEffects _reactionEffects;
		[SerializeField] private PlayerClothesWetness _clothesWetness;
		[SerializeField] private PlayerCompanionOwner _companionOwner;
		[SerializeField] private PlayerCamerasEnabler _camerasEnabler;
		[SerializeField] private PlayerCostumeChanger _costumeChanger;
		[SerializeField] private PlayerVisualEffects _visualEffects;
		[SerializeField] private PlayerCameraShaker _cameraShaker;
		[SerializeField] private PlayerAnimationRig _animationRig;
		[SerializeField] private PlayerSoundEffects _soundEffects;
		[SerializeField] private FatalityTarget _fatalityTarget;
		[SerializeField] private PlayerAnimator _playerAnimator;
		[SerializeField] private PlayerInteractor _interactor;
		[SerializeField] private PlayerRenderer _renderer;
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private PlayerRotator _rotator;
		[SerializeField] private PlayerHealth _health;
		[SerializeField] private Animator _animator;
		[SerializeField] private PlayerMover _mover;
		[SerializeField] private Pausable _pausable;
		[SerializeField] private PlayerJump _jump;
		[SerializeField] private Transform _body;
		[SerializeField] private Player _player;

		private readonly PlayerPursuersHolder _pursuersHolder = new ();
		
		public override void InstallBindings()
		{
			BindToStateMachine();
			BindPausable();
			BindAnimator();
			BindHealth();
			BindCommon();
		}

		private void BindToStateMachine()
		{
			Container.BindInstance(_fatalityTarget).WhenInjectedInto<FatalityReceive>();
			Container.BindInstance(_pursuersHolder).WhenInjectedInto<PlayerState>();
		}
		
		private void BindPausable()
		{
			var list = new List<IPausable>(10)
			{
				_playerAnimator,
			};
			Container.BindInstance(list).WhenInjectedIntoInstance(_pausable);
			Container.Bind<IPausableInfo>().FromInstance(_pausable).AsSingle();
		}
		
		private void BindAnimator()
		{
			Container.Bind<IAnimatorDataChanger>().FromInstance(_playerAnimator)
				.WhenInjectedInto<PlayerCostumeChanger>();
			
			var triggersList = new List<ILayerWithTriggers>(10);
			var layersList = new List<Layer>(10);
			
			BindToAnimator(new BaseLayer(_animator, 0, layersList));
			BindToAnimator(new WeaponLayer(_animator, 1, layersList));
			BindToAnimator(new FullBodyReactionLayer(_animator, 2, layersList));
			BindToAnimator(new RightHandLayer(_animator, 3, layersList, triggersList));
			BindToAnimator(new UpperBodyAdditiveLayer(_animator, 4, layersList));
			BindToAnimator(new FatalityLayer(_animator, 5, layersList));
			
			BindToAnimator(triggersList);
			BindToAnimator(layersList);
			BindToAnimator(_animator);
		}

		private void BindToAnimator<T>(T instance)
			=> Container.BindInstance(instance).WhenInjectedIntoInstance(_playerAnimator);
		
		private void BindHealth()
		{
			Container.Bind<IDamageReceiver>().FromInstance(_health).AsCached();
			Container.Bind<IReadOnlyHealth>().FromInstance(_health).AsCached();
			Container.Bind<IHealReceiver>().FromInstance(_health).AsCached();
			
			Container.BindInstance(_health).WhenInjectedInto<Player>();
		}

		private void BindCommon()
		{
			BindInstanceWhenInjectedInto();
			BindFromInstance();
			BindInstance();
		}
		
		private void BindInstanceWhenInjectedInto()
		{
			BindInstanceToInteractor(_interactableFinder);
			
			Container.BindInstance(_clothesWetnesses).WhenInjectedIntoInstance(_clothesWetness);
			
			Container.BindInstance(_characterController).WhenInjectedIntoInstance(_playerCharacterController);
			Container.BindInstance(_characterController).WhenInjectedIntoInstance(_player);
			Container.BindInstance(_characterController).WhenInjectedIntoInstance(_mover);
			
			Container.BindInstance(_audioSource).WhenInjectedInto<PlayerSoundEffects>();
			Container.BindInstance(_pursuersHolder).WhenInjectedInto<Player>();
		}

		private void BindInstanceToInteractor<T>(T instance) where T : class
			=> Container.BindInstance(instance).WhenInjectedInto<PlayerInteractor>();
		
		private void BindFromInstance()
		{
			Container.Bind<IInteractableFinderUpdater>().FromInstance(_interactableFinder).AsSingle();
			Container.Bind<IReadOnlyFatalityTarget>().FromInstance(_fatalityTarget).AsSingle();
			Container.Bind<IReadOnlyPlayerInteractor>().FromInstance(_interactor).AsSingle();
			Container.Bind<IPlayerCostumeProxy>().FromInstance(_costumeChanger).AsSingle();
			Container.Bind<ICompanionOwner>().FromInstance(_companionOwner).AsSingle();
			Container.Bind<IGroundChecker>().FromInstance(_player).AsSingle();
			Container.Bind<ISpawnable>().FromInstance(_player).AsSingle();
		}
		
		private void BindInstance()
		{
			Container.BindInstance(transform).WithId(CharacterTransformType.Root).AsCached();
			Container.BindInstance(_body).WithId(CharacterTransformType.Body).AsCached();
			Container.BindInstance(_playerCharacterController);
			Container.BindInstance(_puppetMasterHandler);
			Container.BindInstance(_animEventsReceiver);
			Container.BindInstance(_playerAnimator);
			Container.BindInstance(_reactionEffects);
			Container.BindInstance(_camerasEnabler);
			Container.BindInstance(_visualEffects);
			Container.BindInstance(_cameraShaker);
			Container.BindInstance(_animationRig);
			Container.BindInstance(_soundEffects);
			Container.BindInstance(_renderer);
			Container.BindInstance(_rotator);
			Container.BindInstance(_mover);
			Container.BindInstance(_jump);
		}
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_clothesWetnesses.Clear();
			_clothesWetnesses.AddRange(GetComponentsInChildren<ClothesWetness>(true));
			
			_playerCharacterController = GetComponentInChildren<PlayerCharacterController>(true);
			_animEventsReceiver = GetComponentInChildren<PlayerAnimEventsReceiver>(true);
			_interactableFinder = GetComponentInChildren<PlayerInteractableFinder>(true);
			_puppetMasterHandler = GetComponentInChildren<PuppetMasterHandler>(true);
			_characterController = GetComponentInChildren<CharacterController>(true);
			_reactionEffects = GetComponentInChildren<PlayerReactionEffects>(true);
			_camerasEnabler = GetComponentInChildren<PlayerCamerasEnabler>(true);
			_companionOwner = GetComponentInChildren<PlayerCompanionOwner>(true);
			_costumeChanger = GetComponentInChildren<PlayerCostumeChanger>(true);
			_clothesWetness = GetComponentInChildren<PlayerClothesWetness>(true);
			_visualEffects = GetComponentInChildren<PlayerVisualEffects>(true);
			_cameraShaker = GetComponentInChildren<PlayerCameraShaker>(true);
			_soundEffects = GetComponentInChildren<PlayerSoundEffects>(true);
			_animationRig = GetComponentInChildren<PlayerAnimationRig>(true);
			_playerAnimator = GetComponentInChildren<PlayerAnimator>(true);
			_fatalityTarget = GetComponentInChildren<FatalityTarget>(true);
			_interactor = GetComponentInChildren<PlayerInteractor>(true);
			_audioSource = GetComponentInChildren<AudioSource>(true);
			_renderer = GetComponentInChildren<PlayerRenderer>(true);
			_rotator = GetComponentInChildren<PlayerRotator>(true);
			_health = GetComponentInChildren<PlayerHealth>(true);
			_animator = GetComponentInChildren<Animator>(true);
			_mover = GetComponentInChildren<PlayerMover>(true);
			_pausable = GetComponentInChildren<Pausable>(true);
			_jump = GetComponentInChildren<PlayerJump>(true);
			_player = GetComponentInChildren<Player>(true);
			_body = _costumeChanger.transform;
		}
#endif
	}
}