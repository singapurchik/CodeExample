using FAS.Apartments.Inside.Scenarios;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using VInspector;
using Zenject;
using FAS.UI;
using System;

namespace FAS.Apartments.Inside
{
	public class ApartmentInside : ZoneInside, IApartmentWindow
	{
		[SerializeField] private List<SimpleAnimatedLootContainer> _lootContainers = new (10);
		[SerializeField] private CeilingFan _ceilingFan;
		[SerializeField] private Window _window;
		[SerializeField] private Room _bedroom;
		[SerializeField] private Flies _flies;
		[SerializeField] private Room _hall;
		[SerializeField] private UnityEvent _onShowFromWindow;
		
		[Inject] private ApartmentInsideScenarioPlayer _scenarioPlayer;
		[Inject] private IUIScreensGroupSwitcher _uiScreensSwitcher;
		[Inject] private ApartmentCamerasSwitcher _camerasSwitcher;
		[Inject] private Jumpscare _jumpscare;

		private Dictionary<string, SimpleAnimatedLootContainer> _lootContainersDictionary;
		private ApartmentInsideData _currentData;
		
		public bool IsHasJumpscare => _scenarioPlayer.IsScenarioPlaying
		                              && _scenarioPlayer.CurrentScenarioType == ApartmentInsideScenarioType.Geisha;
		
		public event Action OnJumpscareShown;

		private void Awake()
		{
			_lootContainersDictionary = new Dictionary<string, SimpleAnimatedLootContainer>(_lootContainers.Count);
		}

		private void OnEnable()
		{
			foreach (var lootContainer in _lootContainers)
			{
				_lootContainersDictionary.Add(lootContainer.UniqueId, lootContainer);
				lootContainer.OnLooted += OnContainerLooted;
			}
			
			_jumpscare.OnFinishScreamingToWindow += InvokeOnJumpscareShown;
		}

		private void OnDisable()
		{
			foreach (var lootContainer in _lootContainers)
				lootContainer.OnLooted -= OnContainerLooted;
			
			_jumpscare.OnFinishScreamingToWindow -= InvokeOnJumpscareShown;
		}

		public void Setup(ApartmentInsideData data, Texture windowTexture)
		{
			_currentData = data;
			_scenarioPlayer.TryChangeScenario(_currentData.Scenario);

			foreach (var containerData in _currentData.LootContainers)
				if (_lootContainersDictionary.TryGetValue(containerData.ID, out var container))
					container.SetLoot(containerData.LootItem);

			foreach (var lootedContainerID in _currentData.LootedContainerIDs)
				if (_lootContainersDictionary.TryGetValue(lootedContainerID, out var container))
					container.SetLooted();

			_bedroom.Setup(_currentData.BedroomData);
			_hall.Setup(_currentData.HallData);
			_window.SetTexture(windowTexture);
		}

		private void OnContainerLooted(string lootContainerID)
		{
			if (_lootContainersDictionary.TryGetValue(lootContainerID, out var container))
			{
				if (container.IsHasLoot)
					_currentData.RemoveLootContainer(lootContainerID);
				
				_currentData.AddLootedContainerID(lootContainerID);
			}
		}

		public void RotateWindowCamera(float delta) => _window.RotateCamera(delta);
		
		public override void Hide()
		{
			_scenarioPlayer.TryDisassembleScenario();
			_camerasSwitcher.SwitchToHall();
			_window.ResetCameraRotation();
			_window.ShowOutsideParts();
			
			foreach (var lootContainer in _lootContainers)
				lootContainer.Restore();

			base.Hide();
		}

		public void ShowFromHall()
		{
			_scenarioPlayer.TryAssembleScenario();
			base.Show();
			_ceilingFan.ChangeSpatialBlend(1f);
			_flies.ChangeSpatialBlend(1f);
			_camerasSwitcher.SwitchToHall();
		}
		
		public void ShowFromWindow()
		{
			_scenarioPlayer.TryAssembleScenario();
			_ceilingFan.ChangeSpatialBlend(0.5f);
			_flies.ChangeSpatialBlend(0.5f);
			_window.HideOutsideParts();
			base.Show();
			_camerasSwitcher.SwitchToWindow();
			_onShowFromWindow?.Invoke();
		}
		
		private void InvokeOnJumpscareShown()
		{
			print("InvokeOnJumpscareShown");
			OnJumpscareShown?.Invoke();
		}

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_lootContainers.Clear();
			_lootContainers.AddRange(GetComponentsInChildren<SimpleAnimatedLootContainer>(true));
		}
#endif
	}
}