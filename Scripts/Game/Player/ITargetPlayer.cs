namespace FAS.Players
{
	public interface ITargetPlayer : IPlayerInfo
	{
		public IPlayerPursuersHolder PursuersHolder { get; }
		public IDamageReceiver DamageReceiver { get; }
		public IReadOnlyHealth Health { get; }
	}
}