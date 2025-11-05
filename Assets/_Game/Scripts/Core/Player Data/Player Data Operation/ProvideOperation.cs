namespace Core
{
	public abstract class ProvideOperation : PlayerDataOperation
	{
		public override bool IsCanApply(IPlayerDataInfo data) => true;
	}
}