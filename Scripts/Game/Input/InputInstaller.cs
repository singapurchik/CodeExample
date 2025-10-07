using UnityEngine.InputSystem;
using FAS.Players.States;
using FAS.Players;
using UnityEngine;
using VInspector;
using Zenject;
using FAS.UI;

namespace FAS
{
	public class InputInstaller : MonoInstaller
	{
		[SerializeField] private CustomButton _stopLookingIntoWindowButton;
		[SerializeField] private InputActionReference _joystickMoveAction;
		[SerializeField] private MonologueButton _nextMonologueButton;
		[SerializeField] private CustomToggle _changeCostumeButton;
		[SerializeField] private InteractButton _interactButton;
		[SerializeField] private CustomButton _joystickButton;
		[SerializeField] private CustomButton _settingsButton;
		[SerializeField] private CameraInputPanel _inputPanel;
		[SerializeField] private CustomToggle _crouchButton;
		[SerializeField] private CustomButton _shootButton;
		[SerializeField] private Input _input;
		
		[Header("Inventory Model View")]
		[SerializeField] private CustomButton _closeInventoryModelViewButton;
		[SerializeField] private CustomButton _examineInventoryItemButton;
		[SerializeField] private CustomButton _unequipInventoryItemButton;
		[SerializeField] private CustomButton _equipInventoryItemButton;
		[SerializeField] private CustomButton _useInventoryItemButton;
		[SerializeField] private CustomButton _finishAimingButton;
		[SerializeField] private CustomButton _startAimingButton;
		
		public override void InstallBindings()
		{
			var inputEvents = new InputEvents();

			Container.Bind<IMonologueButtonView>().FromInstance(_nextMonologueButton).WhenInjectedInto<Monologue>();
			Container.Bind<IReadOnlyInputEvents>().FromInstance(inputEvents);
			Container.Bind<IInputVisibility>().FromInstance(_input);
			Container.Bind<IReadOnlyInput>().FromInstance(_input);
			Container.Bind<IInputControl>().FromInstance(_input);
			
			BindToInput(_nextMonologueButton);
			BindToInput(_joystickMoveAction);
			BindToInput(_interactButton);
			BindToInput(inputEvents);
			
			BindButtonToInput(_stopLookingIntoWindowButton, InputButtonName.StopLookingIntoWindow);
			BindButtonToInput(_examineInventoryItemButton, InputButtonName.ExamineInventoryItem);
			BindButtonToInput(_unequipInventoryItemButton, InputButtonName.UnequipInventoryItem);
			BindButtonToInput(_equipInventoryItemButton, InputButtonName.EquipInventoryItem);
			BindButtonToInput(_useInventoryItemButton, InputButtonName.UseInventoryItem);
			BindButtonToInput(_changeCostumeButton, InputButtonName.ChangeCostume);
			BindButtonToInput(_finishAimingButton, InputButtonName.FinishAiming);
			BindButtonToInput(_startAimingButton, InputButtonName.StartAiming);
			BindButtonToInput(_joystickButton, InputButtonName.Joystick);
			BindButtonToInput(_settingsButton, InputButtonName.Settings);
			BindButtonToInput(_crouchButton, InputButtonName.Crouch);
			BindButtonToInput(_shootButton, InputButtonName.Shoot);
			
			Container.BindInstance(_inputPanel).WhenInjectedInto<CameraPanelSyncButton>();
			Container.Bind<IReadOnlyCameraInputPanel>().FromInstance(_inputPanel).AsSingle();
		}
		
		private void BindToInput<T>(T instance) where T : class
			=> Container.BindInstance(instance).WhenInjectedInto<Input>();
		
		private void BindButtonToInput<T>(T instance, InputButtonName buttonName) where T : class
			=> Container.BindInstance(instance).WithId(buttonName).WhenInjectedInto<Input>();
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_nextMonologueButton = FindObjectOfType<MonologueButton>(true);
			_interactButton = FindObjectOfType<InteractButton>(true);
			_inputPanel = FindObjectOfType<CameraInputPanel>(true);
			_input = FindObjectOfType<Input>(true);
		}
#endif
	}
}