namespace FAS
{
	public interface IAnimatorDataChanger : IAnimatorDataSaver
	{
		public void ChangeData(AnimatorData data);
	}
}