using FAS.Players.States;
using FAS.Apartments;
using FAS.Players;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.UI
{
	public class UIScreensInstaller : MonoInstaller
	{
		[SerializeField] private UIScreensGroupSwitcher _uiScreensGroupSwitcher;
		[SerializeField] private MonologueScreen _monologueScreen;
		[SerializeField] private PlayerHealthScreen _playerHealthScreen;

		public override void InstallBindings()
		{
			Container.Bind<IInventoryDetailedScreenGroup>().FromInstance(_uiScreensGroupSwitcher)
				.WhenInjectedInto<InventoryDetailed>();
			
			Container.Bind<IInteractionUsedItemScreenGroup>().FromInstance(_uiScreensGroupSwitcher)
				.WhenInjectedInto<InteractionUsedItemDisplayer>();
			
			Container.Bind<IRangeWeaponAimingScreenGroup>()
				.FromInstance(_uiScreensGroupSwitcher).WhenInjectedInto<RangeWeaponState>();
			
			Container.Bind<IApartmentScreensGroup>().FromInstance(_uiScreensGroupSwitcher).WhenInjectedInto<ApartmentZone>();
			Container.Bind<ISettingsScreenGroup>().FromInstance(_uiScreensGroupSwitcher).WhenInjectedInto<Settings>();
			Container.Bind<IPlayerHealthView>().FromInstance(_playerHealthScreen).WhenInjectedInto<PlayerHealthBehaviour>();
			Container.Bind<IMonologueText>().FromInstance(_monologueScreen).WhenInjectedInto<Monologue>();
			
			Container.Bind<IUIScreensGroupSwitcher>().FromInstance(_uiScreensGroupSwitcher).AsSingle();
			Container.Bind<IMonologueScreenGroup>().FromInstance(_uiScreensGroupSwitcher).AsSingle();
		}
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_uiScreensGroupSwitcher = FindObjectOfType<UIScreensGroupSwitcher>(true);
			_monologueScreen = FindObjectOfType<MonologueScreen>(true);
			_playerHealthScreen = FindObjectOfType<PlayerHealthScreen>(true);
		}
#endif
	}
}