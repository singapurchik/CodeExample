using System.Collections.Generic;
using FAS.Apartments;
using System.Linq;
using FAS.Players;
using UnityEngine;

namespace FAS.UI
{
	public class UIScreensGroupSwitcher : MonoBehaviour, IUIScreensGroupSwitcher, ISettingsScreenGroup, IApartmentScreensGroup,
		IInventoryDetailedScreenGroup, IMonologueScreenGroup, IInteractionUsedItemScreenGroup, IRangeWeaponAimingScreenGroup
	{
		[SerializeField] private UIScreen _interactionUsedItemScreen;
		[SerializeField] private UIScreen _rangeWeaponAimingScreen;
		[SerializeField] private UIScreen _inventoryQuickBarScreen;
		[SerializeField] private UIScreen _apartmentWindowScreen;
		[SerializeField] private UIScreen _defaultInputScreen;
		[SerializeField] private UIScreen _monologueScreen;
		[SerializeField] private UIScreen _settingsScreen;
		[SerializeField] private UIScreen _healthScreen;
		
		private readonly HashSet<ScreensGroup> _allScreens = new (10);
		private readonly List<ScreensGroup> _screensStack = new (10);

		private ScreensGroup _rangeWeaponAimingScreens;
		private ScreensGroup _inventoryDetailedScreens;
		private ScreensGroup _apartmentWindowScreens;
		private ScreensGroup _usedItemModelScreens;
		private ScreensGroup _monologueScreens;
		private ScreensGroup _gameplayScreens;
		private ScreensGroup _settingsScreens;
		
		private int _currentScreenGroupIndex = -1;
		
		public IReadOnlyUIScreen InteractionUsedItemScreen => _interactionUsedItemScreen;
		public IReadOnlyUIScreen RangeWeaponAimingScreen => _rangeWeaponAimingScreen;
		public IReadOnlyUIScreen InventoryQuickBarScreen => _inventoryQuickBarScreen;
		public IReadOnlyUIScreen ApartmentWindowScreen => _apartmentWindowScreen;
		public IReadOnlyUIScreen InventoryDetailedScreen { get; private set; }
		public IReadOnlyUIScreen DefaultInputScreen => _defaultInputScreen;
		public IReadOnlyUIScreen MonologueScreen => _monologueScreen;
		public IReadOnlyUIScreen SettingsScreen => _settingsScreen;

		private void Awake()
		{
			var gameplayScreens = new HashSet<UIScreen>
			{
				_defaultInputScreen,
				_inventoryQuickBarScreen
			};
			_gameplayScreens = CreateScreensGroup(gameplayScreens);

			var apartmentWindowsScreens = new HashSet<UIScreen>
			{
				_apartmentWindowScreen
			};
			_apartmentWindowScreens = CreateScreensGroup(apartmentWindowsScreens);

			var settingsScreens = new HashSet<UIScreen>
			{
				_settingsScreen
			};
			_settingsScreens = CreateScreensGroup(settingsScreens);
			
			var monologueScreens = new HashSet<UIScreen>
			{
				_monologueScreen
			};
			_monologueScreens = CreateScreensGroup(monologueScreens);
			
			var usedItemModelScreens = new HashSet<UIScreen>
			{
				_interactionUsedItemScreen
			};
			_usedItemModelScreens = CreateScreensGroup(usedItemModelScreens);
			
			var rangeWeaponAimingScreens = new HashSet<UIScreen>
			{
				_rangeWeaponAimingScreen
			};
			_rangeWeaponAimingScreens = CreateScreensGroup(rangeWeaponAimingScreens);
		}

		private ScreensGroup CreateScreensGroup(HashSet<UIScreen> screens)
		{
			var screensGroup = new ScreensGroup(screens);
			_allScreens.Add(screensGroup);
			return screensGroup;
		}

		private void Start() => Open(_gameplayScreens);
		
		void IInventoryDetailedScreenGroup.Set(UIScreen iventoryDetailedScreen)
		{
			InventoryDetailedScreen = iventoryDetailedScreen;
			var inventoryDetailedScreens = new HashSet<UIScreen>
			{
				_inventoryQuickBarScreen,
				_healthScreen,
				iventoryDetailedScreen
			};
			_inventoryDetailedScreens = CreateScreensGroup(inventoryDetailedScreens);
		}

		void IInventoryDetailedScreenGroup.Show()
		{
			GamePause.TryPause();
			Open(_inventoryDetailedScreens);
		}

		void IInteractionUsedItemScreenGroup.Hide()
		{
			Open(_gameplayScreens);
			GamePause.TryPlay();
		}

		void IInteractionUsedItemScreenGroup.Show()
		{
			GamePause.TryPause();
			Open(_usedItemModelScreens);
		}

		void IInventoryDetailedScreenGroup.Hide()
		{
			Open(_gameplayScreens);
			GamePause.TryPlay();
		}

		void IMonologueScreenGroup.Show()
		{
			GamePause.TryPause();
			Open(_monologueScreens);
		}

		void IApartmentScreensGroup.OpenWindowScreen() => Open(_apartmentWindowScreens);
		
		void IRangeWeaponAimingScreenGroup.Show() => Open(_rangeWeaponAimingScreens);
		
		void ISettingsScreenGroup.Close()
		{
			TryClose(_settingsScreens);
			GamePause.TryPlay();
		}

		void ISettingsScreenGroup.Open()
		{
			GamePause.TryPause();
			Open(_settingsScreens);
		}

		private void Open(ScreensGroup uiScreen)
		{
			if (_screensStack.Contains(uiScreen))
				RemoveFromStack(uiScreen);

			AddToStack(uiScreen);
			UpdateScreenVisibility(uiScreen);
		}

		private void RemoveFromStack(ScreensGroup uiScreen)
		{
			_currentScreenGroupIndex--;
			_screensStack.Remove(uiScreen);
		}

		private void AddToStack(ScreensGroup uiScreen)
		{
			_screensStack.Add(uiScreen);
			_currentScreenGroupIndex++;
		}

		void IUIScreensGroupSwitcher.ShowGameplayScreen() => Open(_gameplayScreens);

		void IUIScreensGroupSwitcher.TryCloseCurrent()
		{
			var currentScreenGroup = _gameplayScreens;

			if (_screensStack.Count > 0)
				RemoveFromStack(_screensStack[_currentScreenGroupIndex]);
				
			if (_screensStack.Count > 0)
				currentScreenGroup = _screensStack[_currentScreenGroupIndex];

			UpdateScreenVisibility(currentScreenGroup);	
		}

		void IUIScreensGroupSwitcher.TryOpenCurrent()
		{
			if (_currentScreenGroupIndex > -1)
				Open(_screensStack[_currentScreenGroupIndex]);
		}

		private void TryClose(ScreensGroup screens)
		{
			if (_screensStack.Contains(screens))
			{
				var currentScreenGroup = _gameplayScreens;

				if (_screensStack[_currentScreenGroupIndex] == screens)
					RemoveFromStack(screens);
				
				if (_screensStack.Count > 0)
					currentScreenGroup = _screensStack[_currentScreenGroupIndex];

				UpdateScreenVisibility(currentScreenGroup);	
			}
		}
		
		private void UpdateScreenVisibility(ScreensGroup current)
		{
			foreach (var screen in _allScreens.Where(screen => screen != current))
				screen.Hide();

			current.Show();
		}
		
		void IUIScreensGroupSwitcher.HideAll()
		{
			foreach (var screen in _allScreens)
				screen.Hide();
		}

	}	
}
