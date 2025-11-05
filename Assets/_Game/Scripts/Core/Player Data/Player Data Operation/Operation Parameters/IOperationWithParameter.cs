namespace Core
{
	public interface IOperationWithParameter
	{
		public IOperationParameter CreateDefaultParam();
		
		public bool IsCanApply(IPlayerDataInfo data, IOperationParameter parameter);
		public bool IsSupports(IOperationParameter parameter);
		
		public void Apply(PlayerData data, IOperationParameter parameter);
	}
}