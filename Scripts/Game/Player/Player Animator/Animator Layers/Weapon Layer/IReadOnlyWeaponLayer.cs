namespace FAS.Players.Animations
{
	public interface IReadOnlyWeaponLayer : IReadOnlyAnimatorLayer
	{
		public int AimingSniperRifleAnimHash { get; }
		public int ReadySniperRifleAnimHash { get; }
		public int StowSniperRifleAnimHash { get; }
		public int ShootSniperRifleAnim { get; }
	}
}