namespace FAS
{
	public interface IDamageReceiver
	{
		public void TryTakeDamage(float damage = 1, DamageDealerType damageDealerType = DamageDealerType.Unknown);
	}
}