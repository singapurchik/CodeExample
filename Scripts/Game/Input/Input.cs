using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;
using UnityEngine;
using FAS.Utils;
using Zenject;
using System;
using FAS.UI;

namespace FAS
{
	public class Input : MonoBehaviour, IReadOnlyInput, IInputControl, IInputVisibility
	{
#if UNITY_EDITOR
		[SerializeField] private InputActionReference _changeCostumeAction;
		[SerializeField] private InputActionReference _interactAction;
#endif
		[Inject(Id = InputButtonName.StopLookingIntoWindow)]
		private CustomButton _stopLookingIntoWindowButton;
		[Inject(Id = InputButtonName.ExamineInventoryItem)]
		private CustomButton _examineInventoryItemButton;
		[Inject(Id = InputButtonName.UnequipInventoryItem)]
		private CustomButton _unequipInventoryItemButton;
		[Inject(Id = InputButtonName.EquipInventoryItem)]
		private CustomButton _equipInventoryItemButton;
		[Inject(Id = InputButtonName.UseInventoryItem)]
		private CustomButton _useInventoryItemButton;
		[Inject(Id = InputButtonName.ChangeCostume)]
		private CustomToggle _changeCostumeButton;
		[Inject(Id = InputButtonName.StartAiming)]
		private CustomButton _startAimingButton;		
		[Inject(Id = InputButtonName.FinishAiming)]
		private CustomButton _finishAimingButton;
		[Inject(Id = InputButtonName.Settings)]
		private CustomButton _settingsButton;
		[Inject(Id = InputButtonName.Joystick)]
		private CustomButton _joystickButton;
		[Inject(Id = InputButtonName.Crouch)]
		private CustomToggle _crouchButton;
		[Inject(Id = InputButtonName.Shoot)]
		private CustomButton _shootButton;
		
		[Inject] private InputActionReference _joystickMoveAction;
		[Inject] private MonologueButton _nextMonologueButton;
		[Inject] private InteractButton _interactButton;
		[Inject] private InputEvents _events;
		[Inject] private Camera _mainCamera;

		private Quaternion _cameraRotationWhenStartedInput;
		private Vector2 _currentMousePosition;
		private Vector2 _lastMousePosition;

		private float _lastMouseTime;

		private bool _isHoldingJoystickInput;

		public Vector2 MouseVelocity { get; private set; }
		public Vector2 MouseDelta { get; private set; }

		public bool IsCrouchButtonEnabled => !_crouchButton.IsEnabled;
		public bool IsMovementJoystickActive { get; private set; } = true;
		public bool IsButtonsInputActive { get; private set; } = true;
		public bool IsMouseButtonDown { get; private set; }
		public bool IsMouseButtonUp { get; private set; }
		public bool IsMouseHeld { get; private set; }

		private void OnEnable()
		{
			_stopLookingIntoWindowButton.OnButtonDown.AddListener(OnStopLookingIntoWindowButtonClicked);
			_examineInventoryItemButton.OnButtonDown.AddListener(OnExamineInventoryItemButtonClicked);
			_unequipInventoryItemButton.OnButtonDown.AddListener(OnUnequipInventoryItemButtonClicked);
			_equipInventoryItemButton.OnButtonDown.AddListener(OnEquipInventoryItemButtonClicked);
			_useInventoryItemButton.OnButtonDown.AddListener(OnUseInventoryItemButtonClicked);
			_changeCostumeButton.OnButtonDown.AddListener(OnChangeCostumeButtonClicked);
			_nextMonologueButton.OnButtonDown.AddListener(OnNextMonologueButtonClicked);
			_joystickButton.OnButtonDown.AddListener(_events.InvokeOnAnyButtonClicked);
			_finishAimingButton.OnClick.AddListener(OnFinishAimingButtonClicked);
			_startAimingButton.OnClick.AddListener(OnStartAimingButtonClicked);
			_settingsButton.OnButtonDown.AddListener(OnSettingsButtonClicked);
			_interactButton.OnButtonDown.AddListener(OnInteractButtonClicked);
			_shootButton.OnButtonDown.AddListener(OnShootButtonClicked);
			_crouchButton.OnClick.AddListener(OnCrouchButtonClicked);
			_joystickMoveAction.action.Enable();

#if UNITY_EDITOR
			_interactAction?.action.Enable();
#endif
		}

		private void OnDisable()
		{
			_stopLookingIntoWindowButton.OnButtonDown.RemoveListener(OnStopLookingIntoWindowButtonClicked);
			_examineInventoryItemButton.OnButtonDown.RemoveListener(OnExamineInventoryItemButtonClicked);
			_unequipInventoryItemButton.OnButtonDown.RemoveListener(OnUnequipInventoryItemButtonClicked);
			_equipInventoryItemButton.OnButtonDown.RemoveListener(OnEquipInventoryItemButtonClicked);
			_useInventoryItemButton.OnButtonDown.RemoveListener(OnUseInventoryItemButtonClicked);
			_changeCostumeButton.OnButtonDown.RemoveListener(OnChangeCostumeButtonClicked);
			_nextMonologueButton.OnButtonDown.RemoveListener(OnNextMonologueButtonClicked);
			_joystickButton.OnButtonDown.RemoveListener(_events.InvokeOnAnyButtonClicked);
			_finishAimingButton.OnClick.RemoveListener(OnFinishAimingButtonClicked);
			_startAimingButton.OnClick.RemoveListener(OnStartAimingButtonClicked);
			_settingsButton.OnButtonDown.RemoveListener(OnSettingsButtonClicked);
			_interactButton.OnButtonDown.RemoveListener(OnInteractButtonClicked);
			_shootButton.OnButtonDown.RemoveListener(OnShootButtonClicked);
			_crouchButton.OnClick.RemoveListener(OnCrouchButtonClicked);
			_joystickMoveAction.action.Disable();

#if UNITY_EDITOR
			_interactAction?.action.Disable();
#endif
		}

		private void OnFinishAimingButtonClicked()
			=> TrySendOnButtonClickEvent(_events.InvokeOnFinishAimingButtonClicked);
		
		private void OnStartAimingButtonClicked()
			=> TrySendOnButtonClickEvent(_events.InvokeOnStartAimingButtonClicked);
		
		private void OnShootButtonClicked()
			=> TrySendOnButtonClickEvent(_events.InvokeOnShootButtonClicked);

		private void OnChangeCostumeButtonClicked()
		{
			if (_changeCostumeButton.IsEnabled)
				TrySendOnButtonClickEvent(_events.InvokeOnSetGirlCostumeButtonClicked);
			else
				TrySendOnButtonClickEvent(_events.InvokeOnSetGuyCostumeButtonClicked);
		}

		private void OnCrouchButtonClicked()
		{
			if (_crouchButton.IsEnabled)
				TrySendOnButtonClickEvent(_events.InvokeOnCrouchButtonClicked);
			else
				TrySendOnButtonClickEvent(_events.InvokeOnStandButtonClicked);
		}

		private void OnUnequipInventoryItemButtonClicked()
			=> TrySendOnButtonClickEvent(_events.InvokeOnUnequipInventoryItemButtonClicked);

		private void OnEquipInventoryItemButtonClicked()
			=> TrySendOnButtonClickEvent(_events.InvokeOnEquipInventoryItemButtonClicked);

		private void OnUseInventoryItemButtonClicked()
			=> TrySendOnButtonClickEvent(_events.InvokeOnUseInventoryItemButtonClicked);

		private void OnExamineInventoryItemButtonClicked()
			=> TrySendOnButtonClickEvent(_events.InvokeOnExamineInventoryItemButtonClicked);

		private void OnStopLookingIntoWindowButtonClicked()
			=> TrySendOnButtonClickEvent(_events.InvokeOnStopLookingIntoWindowButtonClicked);

		private void OnNextMonologueButtonClicked()
			=> TrySendOnButtonClickEvent(_events.InvokeOnNextMonologueButtonClicked);

		private void OnSettingsButtonClicked() => TrySendOnButtonClickEvent(_events.InvokeOnSettingsButtonClicked);

		private void OnInteractButtonClicked() => TrySendOnButtonClickEvent(_events.InvokeOnInteractButtonClicked);

		public void DisableMovementInput() => IsMovementJoystickActive = false;

		public void EnableMovementInput() => IsMovementJoystickActive = true;

		public void DisableButtonsInput() => IsButtonsInputActive = false;

		public void EnableButtonsInput() => IsButtonsInputActive = true;

		private void TrySendOnButtonClickEvent(Action action)
		{
			if (IsButtonsInputActive)
				action?.Invoke();
		}

		public void TryHideInteractButton() => _interactButton.gameObject.TryDisable();

		public void TryShowInteractButton(Sprite icon)
		{
			_interactButton.SetInteractionIcon(icon);
			_interactButton.gameObject.TryEnable();
		}

		public void TryHideInventoryExamineItemButton() => _examineInventoryItemButton.gameObject.TryDisable();

		public void TryShowInventoryExamineItemButton() => _examineInventoryItemButton.gameObject.TryEnable();

		public void TryHideInventoryUnequipItemButton() => _unequipInventoryItemButton.gameObject.TryDisable();

		public void TryShowInventoryUnequipItemButton() => _unequipInventoryItemButton.gameObject.TryEnable();

		public void TryHideInventoryEquipItemButton() => _equipInventoryItemButton.gameObject.TryDisable();

		public void TryShowInventoryEquipItemButton() => _equipInventoryItemButton.gameObject.TryEnable();

		public void TryHideUseInventoryItemButton() => _useInventoryItemButton.gameObject.TryDisable();

		public void TryShowUseInventoryItemButton() => _useInventoryItemButton.gameObject.TryEnable();

		public Vector3 GetJoystickDirection3D()
		{
			return ActiveCameraData.MoveType == CameraMoveType.Fixed
				? GetFixedCameraDirection()
				: GetDynamicCameraDirection();
		}

		private Vector3 GetDynamicCameraDirection()
		{
			var movementDirection = Vector3.zero;
			if (IsMovementJoystickActive)
			{
				var v = _joystickMoveAction.action.ReadValue<Vector2>();
				movementDirection.x = v.x;
				movementDirection.z = v.y;
				movementDirection = _mainCamera.transform.TransformDirection(movementDirection);
				movementDirection.y = 0f;
			}

			return movementDirection.normalized;
		}

		private Vector3 GetFixedCameraDirection()
		{
			if (IsMovementJoystickActive)
			{
				var rawInput = _joystickMoveAction.action.ReadValue<Vector2>();
				if (rawInput.sqrMagnitude > 0.01f)
				{
					if (!_isHoldingJoystickInput)
					{
						_cameraRotationWhenStartedInput = Quaternion.Euler(0f, _mainCamera.transform.eulerAngles.y, 0f);
						_isHoldingJoystickInput = true;
					}

					var worldDir = _cameraRotationWhenStartedInput * new Vector3(rawInput.x, 0f, rawInput.y);
					worldDir.y = 0f;
					return worldDir.normalized;
				}

				_isHoldingJoystickInput = false;
			}

			return Vector3.zero;
		}

		public Vector2 GetJoystickDirection2D()
		{
			var moveVector = Vector2.zero;
			if (IsMovementJoystickActive)
				moveVector = _joystickMoveAction.action.ReadValue<Vector2>();
			return moveVector;
		}

		private void GetPointerInput(out Vector2 position, out ButtonControl button)
		{
#if UNITY_EDITOR
			button = Mouse.current.leftButton;
			position = Mouse.current.position.ReadValue();
#elif UNITY_ANDROID || UNITY_IOS
			button = Touchscreen.current.primaryTouch.press;
			position = Touchscreen.current.primaryTouch.position.ReadValue();
#endif
		}

		private void UpdatePointerDrag(Vector2 pos, bool isPressed)
		{
			if (IsMouseButtonDown)
			{
				IsMouseHeld = true;
				_lastMousePosition = pos;
				_currentMousePosition = pos;
				_lastMouseTime = Time.unscaledTime;
				MouseDelta = Vector2.zero;
				MouseVelocity = Vector2.zero;
			}
			
			if (IsMouseHeld && isPressed)
			{
				_currentMousePosition = pos;
				MouseDelta = _currentMousePosition - _lastMousePosition;
				float deltaTime = Time.unscaledTime - _lastMouseTime;
				MouseVelocity = deltaTime > 0f ? MouseDelta / deltaTime : Vector2.zero;
				_lastMousePosition = _currentMousePosition;
				_lastMouseTime = Time.unscaledTime;
			}
			
			if (IsMouseButtonUp)
			{
				IsMouseHeld = false;
				MouseDelta = Vector2.zero;
				MouseVelocity = Vector2.zero;
			}
		}

		private void Update()
		{
			GetPointerInput(out var position, out var button);
			IsMouseButtonDown = button.wasPressedThisFrame;
			IsMouseButtonUp = button.wasReleasedThisFrame;
			bool isPressed = button.isPressed;
			UpdatePointerDrag(position, isPressed);

#if UNITY_EDITOR
			PollKeyboardShortcuts();
#endif
		}

#if UNITY_EDITOR
		private void PollKeyboardShortcuts()
		{
			if (IsButtonsInputActive)
			{
				if (_changeCostumeAction.action.WasPressedThisFrame())
				{
					OnChangeCostumeButtonClicked();

					if (_changeCostumeButton.IsEnabled)
						_changeCostumeButton.SilentDisable();
					else
						_changeCostumeButton.SilentEnable();
				}

				if (_interactAction.action.WasPressedThisFrame())
					_events.InvokeOnInteractButtonClicked();
			}
		}
#endif
	}
}