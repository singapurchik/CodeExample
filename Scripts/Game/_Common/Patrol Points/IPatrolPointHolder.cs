namespace FAS
{
	public interface IPatrolPointHolder<out T> where T : IReadOnlyPatrolPoint
	{
		public IReadOnlyPatrolPoint Current { get;}
		public IReadOnlyPatrolPoint Next { get;}
	}
}