using UnityEngine;
using Zenject;

namespace FAS.Actors.Emenies
{
	public class EnemyStateMachine : StateMachine<EnemyStates, EnemyState>, IEnemyStateMachineInfo
	{
		[SerializeField] private bool _isUsePatrolState;
		
		[Inject] private EnemyAnimator _animator;
		
		[Inject] private FatalityExecute _fatalityExecuteState;
		[Inject] private ShowingFace _showingFaceState;
		[Inject] private Patrol _patrolState;
		[Inject] private Attack _attackState;
		[Inject] private Chase _chaseState;
		[Inject] private Idle _idleState;

		public bool IsUsePatrolState => _isUsePatrolState;
		
		protected override void OnDisable()
		{
			TrySwitchStateTo(_idleState);
		}

		public override void Initialize()
		{
			AddState(_fatalityExecuteState);
			AddState(_showingFaceState);
			AddState(_patrolState);
			AddState(_attackState);
			AddState(_chaseState);
			AddState(_idleState);
			
			TrySwitchStateTo(_idleState);
		}

		protected override void ExitCurrentState()
		{
			base.ExitCurrentState();
			_animator.TryResetTriggers();
		}
	}
}