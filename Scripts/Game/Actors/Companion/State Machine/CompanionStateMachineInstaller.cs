using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Actors.Companion
{
	public class CompanionStateMachineInstaller : MonoInstaller
	{
		[SerializeField] private TeleportToPlayer _teleportToPlayerState;
		[SerializeField] private CompanionStateMachine _stateMachine;
		[SerializeField] private StandUpFromBed _standUpFromBedState;
		[SerializeField] private ChillingOnBed _chillingOnBedState;
		[SerializeField] private FollowPlayer _followPlayerState;
		[SerializeField] private KnockedBack _knockedBackState;
		[SerializeField] private Idle _idleState;
		
		public override void InstallBindings()
		{
			Container.BindInstance(_stateMachine).WhenInjectedInto<Companion>();
			
			BindInstanceToStateMachine(_teleportToPlayerState);
			BindInstanceToStateMachine(_standUpFromBedState);
			BindInstanceToStateMachine(_chillingOnBedState);
			BindInstanceToStateMachine(_followPlayerState);
			BindInstanceToStateMachine(_knockedBackState);
			BindInstanceToStateMachine(_idleState);
		}
		
		private void BindInstanceToStateMachine<T>(T instance)
			=> Container.BindInstance(instance).WhenInjectedInto<CompanionStateMachine>();
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_teleportToPlayerState = GetComponentInChildren<TeleportToPlayer>(true);
			_stateMachine = GetComponentInChildren<CompanionStateMachine>(true);
			_standUpFromBedState = GetComponentInChildren<StandUpFromBed>(true);
			_chillingOnBedState = GetComponentInChildren<ChillingOnBed>(true);
			_followPlayerState = GetComponentInChildren<FollowPlayer>(true);
			_knockedBackState = GetComponentInChildren<KnockedBack>(true);
			_idleState = GetComponentInChildren<Idle>(true);
		}
#endif
	}
}