using Camera = UnityEngine.Camera;
using FAS.Apartments.Inside;
using FAS.Actors.Companion;
using FAS.Fatality;
using FAS.Players;
using UnityEngine;
using VInspector;
using FAS.Items;
using Zenject;
using FAS.UI;

namespace FAS.Sandbox
{
	public class GameSceneInstaller : MonoInstaller
	{
		[SerializeField] private PlayerCameraShaker _playerCameraShaker;
		[SerializeField] private FadingFrameScreen _fadingFrameScreen;
		[SerializeField] private PlayerCostumeChanger _costumeChanger;
		[SerializeField] private ParticleSystem _cameraBloodEffect;
		[SerializeField] private FatalityTarget _fatalityTarget;
		[SerializeField] private SettingsView _settingsView;
		[SerializeField] private ItemDropper _itemDropper;
		[SerializeField] private Companion _companion;
		[SerializeField] private Camera _mainCamera;
		[SerializeField] private Player _player;
		
		public override void InstallBindings()
		{
			Container.Bind<IUIFadingFrame>().FromInstance(_fadingFrameScreen).WhenInjectedInto<PlayerVisualEffects>();
			Container.Bind<ParticleSystem>().FromInstance(_cameraBloodEffect).WhenInjectedInto<PlayerVisualEffects>();
			Container.Bind<IUIFadingFrame>().FromInstance(_fadingFrameScreen).WhenInjectedInto<Jumpscare>();
			Container.Bind<ISettingView>().FromInstance(_settingsView).WhenInjectedInto<Settings>();
			Container.Bind<IPlayerCostumeProxy>().FromInstance(_costumeChanger).AsSingle();
			Container.Bind<ICameraShaker>().FromInstance(_playerCameraShaker).AsSingle();
			Container.Bind<IFatalityTarget>().FromInstance(_fatalityTarget).AsSingle();
			Container.Bind<ICompanion>().FromInstance(_companion).AsSingle();
			Container.Bind<IPlayerInfo>().FromInstance(_player).AsSingle();
			Container.Bind<Camera>().FromInstance(_mainCamera).AsSingle();

			Container.BindInstance(_itemDropper).WhenInjectedInto<LootContainer>();
			Container.Bind<IItemDropBonus>().FromInstance(_itemDropper).AsSingle();
		}
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_playerCameraShaker = FindObjectOfType<PlayerCameraShaker>(true);
			_costumeChanger = FindObjectOfType<PlayerCostumeChanger>(true);
			_fadingFrameScreen = FindObjectOfType<FadingFrameScreen>(true);
			_fatalityTarget = FindObjectOfType<FatalityTarget>(true);
			_settingsView = FindObjectOfType<SettingsView>(true);
			_itemDropper = FindObjectOfType<ItemDropper>(true);
			_companion = FindObjectOfType<Companion>(true);
			_player = FindObjectOfType<Player>(true);
			_mainCamera = Camera.main;
		}
#endif
	}
}