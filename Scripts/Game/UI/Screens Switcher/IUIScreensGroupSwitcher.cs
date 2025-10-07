namespace FAS.UI
{
	public interface IUIScreensGroupSwitcher
	{
		public void ShowGameplayScreen();
		
		public void TryCloseCurrent();
		
		public void TryOpenCurrent();
		
		public void HideAll();

		public IReadOnlyUIScreen InteractionUsedItemScreen { get; }
		public IReadOnlyUIScreen RangeWeaponAimingScreen { get; }
		public IReadOnlyUIScreen InventoryQuickBarScreen { get; }
		public IReadOnlyUIScreen InventoryDetailedScreen { get;}
		public IReadOnlyUIScreen ApartmentWindowScreen { get; }
		public IReadOnlyUIScreen DefaultInputScreen { get; }
		public IReadOnlyUIScreen MonologueScreen { get; }
		public IReadOnlyUIScreen SettingsScreen { get; }
	}
}