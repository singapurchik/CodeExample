namespace FAS.Actors.Emenies
{
	public interface IEnemyStateMachineInfo : IStateMachineInfo<EnemyStates>
	{
		public bool IsUsePatrolState { get; }
	}
}