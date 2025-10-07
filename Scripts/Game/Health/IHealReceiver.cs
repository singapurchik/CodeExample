namespace FAS
{
	public interface IHealReceiver
	{
		public void TryHeal(float heal = 1, HealDealerType healDealerType = HealDealerType.Unknown);
	}
}