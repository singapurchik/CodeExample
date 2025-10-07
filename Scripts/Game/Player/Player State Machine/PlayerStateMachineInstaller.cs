using FAS.Players.States.Interaction;
using FAS.Players.States.Triggers;
using FAS.Players.States.Zone;
using FAS.Players.States;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Players
{
	public class PlayerStateMachineInstaller : MonoInstaller
	{
		[SerializeField] private MoveFromExitToStartCorridor _moveFromExitToStartCorridorState;
		[SerializeField] private InteractionWithLootHolder _interactionWithLootHolderState;
		[SerializeField] private InteractionWithDeadBody _interactionWithDeadBodyState;
		[SerializeField] private RangeWeaponReloading _rangeWeaponReloadingState;
		[SerializeField] private RangeWeaponShooting _rangeWeaponShootingState;
		[SerializeField] private RotateToInteraction _rotateToInteractionState;
		[SerializeField] private TeleportTransition _teleportTransitionState;
		[SerializeField] private MoveToInteraction _moveToInteractionState;
		[SerializeField] private RangeWeaponAiming _rangeWeaponAimingState;
		[SerializeField] private RangeWeaponReady _rangeWeaponReadyState;
		[SerializeField] private RangeWeaponStow _rangeWeaponStowState;
		[SerializeField] private FatalityReceive _fatalityReceiveState;
		[SerializeField] private LookTransition _lookTransitionState;
		[SerializeField] private PlayerStateMachine _stateMachine;
		[SerializeField] private KnockedDown _knockedDownState;
		[SerializeField] private LookAtZone _lookAtZoneState;
		[SerializeField] private Monologue _monologueState;
		[SerializeField] private Respawn _respawnState;
		[SerializeField] private PlayerWeapon _weapon;
		[SerializeField] private Death _deathState;
		[SerializeField] private Free _freeState;

		public override void InstallBindings()
		{
			BindInstanceToStateMachine(_moveFromExitToStartCorridorState);
			BindInstanceToStateMachine(_interactionWithLootHolderState);
			BindInstanceToStateMachine(_interactionWithDeadBodyState);
			BindInstanceToStateMachine(_rangeWeaponReloadingState);
			BindInstanceToStateMachine(_rangeWeaponShootingState);
			BindInstanceToStateMachine(_rotateToInteractionState);
			BindInstanceToStateMachine(_teleportTransitionState);
			BindInstanceToStateMachine(_moveToInteractionState);
			BindInstanceToStateMachine(_rangeWeaponAimingState);
			BindInstanceToStateMachine(_rangeWeaponReadyState);
			BindInstanceToStateMachine(_fatalityReceiveState);
			BindInstanceToStateMachine(_rangeWeaponStowState);
			BindInstanceToStateMachine(_lookTransitionState);
			BindInstanceToStateMachine(_knockedDownState);
			BindInstanceToStateMachine(_lookAtZoneState);
			BindInstanceToStateMachine(_monologueState);
			BindInstanceToStateMachine(_respawnState);
			BindInstanceToStateMachine(_deathState);
			BindInstanceToStateMachine(_freeState);

			Container.Bind<IWeaponStatesSwitcher>().FromInstance(_stateMachine).WhenInjectedIntoInstance(_weapon);
			Container.Bind<IPlayerStateReturner>().FromInstance(_stateMachine).WhenInjectedInto<PlayerState>();

			Container.Bind<IPlayerStateMachineInfo>().FromInstance(_stateMachine).AsSingle();
			
			Container.Bind<IInteractionStatesSwitcher>().FromInstance(_stateMachine)
				.WhenInjectedInto<PlayerInteractor>();
			
			Container.Bind<ITriggerStatesSwitcher>().FromInstance(_stateMachine)
				.WhenInjectedInto<PlayerTriggerVisitor>();

			Container.Bind<IPlayerRangeWeapon>().FromInstance(_weapon).WhenInjectedInto<RangeWeaponState>();
			Container.BindInstance(_stateMachine).WhenInjectedInto<Player>();
		}
		
		private void BindInstanceToStateMachine<T>(T instance)
			=> Container.BindInstance(instance).WhenInjectedInto<PlayerStateMachine>();
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_weapon = transform.parent.GetComponentInChildren<PlayerWeapon>(true);
			
			_moveFromExitToStartCorridorState = GetComponentInChildren<MoveFromExitToStartCorridor>(true);
			_interactionWithLootHolderState = GetComponentInChildren<InteractionWithLootHolder>(true);
			_interactionWithDeadBodyState = GetComponentInChildren<InteractionWithDeadBody>(true);
			_rangeWeaponReloadingState = GetComponentInChildren<RangeWeaponReloading>(true);
			_rangeWeaponShootingState = GetComponentInChildren<RangeWeaponShooting>(true);
			_rotateToInteractionState = GetComponentInChildren<RotateToInteraction>(true);
			_teleportTransitionState = GetComponentInChildren<TeleportTransition>(true);
			_moveToInteractionState = GetComponentInChildren<MoveToInteraction>(true);
			_rangeWeaponAimingState = GetComponentInChildren<RangeWeaponAiming>(true);
			_rangeWeaponReadyState = GetComponentInChildren<RangeWeaponReady>(true);
			_fatalityReceiveState = GetComponentInChildren<FatalityReceive>(true);
			_rangeWeaponStowState = GetComponentInChildren<RangeWeaponStow>(true);
			_lookTransitionState = GetComponentInChildren<LookTransition>(true);
			_stateMachine = GetComponentInChildren<PlayerStateMachine>(true);
			_knockedDownState = GetComponentInChildren<KnockedDown>(true);
			_lookAtZoneState = GetComponentInChildren<LookAtZone>(true);
			_monologueState = GetComponentInChildren<Monologue>(true);
			_respawnState = GetComponentInChildren<Respawn>(true);
			_deathState = GetComponentInChildren<Death>(true);
			_freeState = GetComponentInChildren<Free>(true);
		}
#endif
	}
}