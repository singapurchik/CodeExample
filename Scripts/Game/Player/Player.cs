using FAS.Players.Animations;
using FAS.Players.States;
using UnityEngine;
using FAS.Sounds;
using Zenject;

namespace FAS.Players
{
	public class Player : MonoBehaviour, ISpawnable, ITargetPlayer, IGroundChecker, ICinemachineCameraTarget
	{
		[Inject] private CharacterController _characterController;
		[Inject] private PlayerAnimEventsReceiver _animatorEvents;
		[Inject] private PlayerPursuersHolder _pursuersHolder;
		[Inject] private IPlayerCostumeProxy _costumeProxy;
		[Inject] private IReadOnlyInputEvents _inputEvents;
		[Inject] private PlayerSoundEffects _soundEffects;
		[Inject] private PlayerStateMachine _stateMachine;
		[Inject] private IMusicChanger _musicChanger;
		[Inject] private IReadOnlyInput _input;
		[Inject] private PlayerHealth _health;
		
		public HumanoidBonesHolder Bones => _costumeProxy.Data.BonesHolder;
		public IPlayerPursuersHolder PursuersHolder => _pursuersHolder;
		public IDamageReceiver DamageReceiver => _health;
		public IReadOnlyHealth Health => _health;
		
		public Transform PlayerTransform => transform;

		public Vector3 EulersAngles => transform.eulerAngles;
		public Vector3 FollowPosition => transform.position;
		public Vector3 SpawnPosition { get; private set; }
		public Vector3 Position => transform.position;
		
		public bool IsGrounded => _characterController.isGrounded;
		public bool IsCrouched => !_input.IsCrouchButtonEnabled;
		
		private void OnEnable()
		{
			_pursuersHolder.OnFirstPursuiterAdded += _musicChanger.PlayPursuitMusic;
			_pursuersHolder.OnLastPursuiterRemoved += OnLastPursuiterRemoved;
			_animatorEvents.OnFootstepWalk += OnFootstepWalk;
			_animatorEvents.OnFootstepRun += OnFootstepRun;
		}

		private void OnDisable()
		{
			_pursuersHolder.OnFirstPursuiterAdded -= _musicChanger.PlayPursuitMusic;
			_pursuersHolder.OnLastPursuiterRemoved -= OnLastPursuiterRemoved;
			_animatorEvents.OnFootstepWalk -= OnFootstepWalk;
			_animatorEvents.OnFootstepRun -= OnFootstepRun;
		}

		private void Start()
		{
			SpawnPosition = transform.position;
			_stateMachine.Initialize();
		}
		
		private void OnLastPursuiterRemoved() => _musicChanger.PlayStreetBackgroundMusic(true);

		private void OnFootstepWalk() => _soundEffects.PlayFootstepsWalk();
		
		private void OnFootstepRun() => _soundEffects.PlayFootstepsRun();
	}
}