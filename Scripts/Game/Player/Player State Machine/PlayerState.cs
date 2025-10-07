using FAS.Players.Animations;
using FAS.Players.AnimRig;
using FAS.Fatality;
using UnityEngine;
using Zenject;
using FAS.UI;

namespace FAS.Players.States
{
	public enum PlayerStates
	{
		Undefined = 0,
		InteractionWithLootContainer = 1,
		LookTransition = 2,
		SwitchToMoveFromExitToStartCorridor = 3,
		TeleportTransition = 4,
		MoveToInteraction = 5,
		FatalityReceive = 6,
		KnockedDown = 7,
		Monologue = 8,
		Respawn = 9,
		LookAtZone = 10,
		Free = 11,
		Death = 12,
		InteractionWithDeadBody = 13,
		RotateToInteraction = 14,
		RangeWeaponReady = 15,
		RangeWeaponAiming = 16,
		RangeWeaponStow = 17,
		RangeWeaponShooting = 18,
		RangeWeaponReloading = 19,
	}

	public abstract class PlayerState : State<PlayerStates, PlayerStates>
	{
		[Inject (Id = CharacterTransformType.Root)] protected Transform Transform;
		[Inject (Id = CharacterTransformType.Body)] protected Transform Body;
		[Inject] protected IInteractableFinderUpdater InteractableFinderUpdater;
		[Inject] protected PlayerCharacterController CharacterController;
		[Inject] protected ICinemachineBrainInfo CinemachineBrainInfo;
		[Inject] protected IUIScreensGroupSwitcher UIScreensSwitcher;
		[Inject] protected ICameraBlendsChanger CameraBlendsChanger;
		[Inject] protected IFollowCameraRotator FollowCameraRotator;
		[Inject] protected IReadOnlyFatalityTarget FatalityTarget;
		[Inject] protected IPlayerStateMachineInfo StateMachine;
		[Inject] protected PlayerPursuersHolder PursuersHolder;
		[Inject] protected PlayerCamerasEnabler CamerasEnabler;
		[Inject] protected IReadOnlyPlayerInteractor Interactor;
		[Inject] protected PlayerAnimEventsReceiver AnimEvents;
		[Inject] protected IPlayerStateReturner StateReturner;
		[Inject] protected PlayerVisualEffects VisualEffects;
		[Inject] protected IInputVisibility InputVisibility;
		[Inject] protected IReadOnlyInputEvents InputEvents;
		[Inject] protected IPlayerCostumeProxy CostumeProxy;
		[Inject] protected PlayerAnimationRig AnimationRig;
		[Inject] protected PlayerCameraShaker CameraShaker;
		[Inject] protected PlayerSoundEffects SoundEffects;
		[Inject] protected IGroundChecker GroundChecker;
		[Inject] protected IInputControl InputControl;
		[Inject] protected GlobalInput GlobalInput;
		[Inject] protected PlayerAnimator Animator;
		[Inject] protected PlayerRenderer Renderer;
		[Inject] protected IPausableInfo Pausable;
		[Inject] protected PlayerRotator Rotator;
		[Inject] protected IReadOnlyInput Input;
		[Inject] protected ISpawnable Spawnable;
		[Inject] protected PlayerMover Mover;
		[Inject] protected PlayerJump Jump;

		public abstract bool IsPlayerControlledState { get; }

		protected IFakeShadow FakeShadow => CostumeProxy.Data.FakeShadow;
		protected Transform Parent { get; private set; }

		protected virtual void Awake() => Parent = transform.parent;
	}
}