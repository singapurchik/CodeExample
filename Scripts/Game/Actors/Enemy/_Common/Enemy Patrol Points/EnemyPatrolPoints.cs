using VInspector;

namespace FAS.Actors.Emenies
{
	public class EnemyPatrolPoints : PatrolPoints<EnemyPatrolPoint>, IEnemyPatrolPointHolder
	{
#if UNITY_EDITOR
		[Button]
		private void FindPoints()
		{
			Points.Clear();
			Points.AddRange(GetComponentsInChildren<EnemyPatrolPoint>(true));
		}
#endif
	}
}