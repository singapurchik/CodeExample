namespace FAS
{
	public interface IClothesWetness
	{
		public void RequestChangeDisableWetnessSpeed(float speed);

		public void RequestEnableWetnessSmooth(float speed = 0);
		
		public void RequestForceEnableWetness();
		
		public float CurrentWetAmount { get; }
	}
}