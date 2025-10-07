using UnityEngine.UI;
using UnityEngine;
using System;
using FAS.UI;
using TMPro;

namespace FAS
{
	public class SettingsView : UIInteractableScreen, ISettingView
	{
		[SerializeField] private SelectableButton _lowQualityButton;
		[SerializeField] private SelectableButton _normalQualityButton;
		[SerializeField] private SelectableButton _highQualityButton;
		[SerializeField] private SelectableButton _ultraQualityButton;
		[Space(5)]
		[SerializeField] private SelectableButton _120FPSButton;
		[SerializeField] private SelectableButton _90FPSButton;
		[SerializeField] private SelectableButton _60FPSButton;
		[SerializeField] private SelectableButton _30FPSButton;
		[Space(5)]
		[SerializeField] private Slider _sensitivitySlider;
		[SerializeField] private TextMeshProUGUI _sensitivityValueText;
		[Space(5)]
		[SerializeField] private CustomButton _closeButton;
		
		public event Action<float> OnSensitivitySliderValueChanged;
		public event Action OnNormalQualityButtonClicked;
		public event Action OnUltraQualityButtonClicked;
		public event Action OnHighQualityButtonClicked;
		public event Action OnLowQualityButtonClicked;
		public event Action On120FPSButtonClicked;
		public event Action On30FPSButtonClicked;
		public event Action On60FPSButtonClicked;
		public event Action On90FPSButtonClicked;
		public event Action OnCloseButtonClicked;

		private void OnEnable()
		{
			_sensitivitySlider.onValueChanged.AddListener(InvokeOnSensitivitySliderValueChanged);
			_normalQualityButton.OnClick.AddListener(InvokeOnNormalQualityButtonClicked);
			_ultraQualityButton.OnClick.AddListener(InvokeOnUltraQualityButtonClicked);
			_highQualityButton.OnClick.AddListener(InvokeOnHighQualityButtonClicked);
			_lowQualityButton.OnClick.AddListener(InvokeOnLowQualityButtonClicked);
			_120FPSButton.OnClick.AddListener(InvokeOn120FPSButtonClicked);
			_30FPSButton.OnClick.AddListener(InvokeOn30FPSButtonClicked);
			_60FPSButton.OnClick.AddListener(InvokeOn60FPSButtonClicked);
			_90FPSButton.OnClick.AddListener(InvokeOn90FPSButtonClicked);
			_closeButton.OnClick.AddListener(InvokeOnCloseButtonClicked);
		}
		
		private void OnDisable()
		{
			_sensitivitySlider.onValueChanged.RemoveListener(InvokeOnSensitivitySliderValueChanged);
			_normalQualityButton.OnClick.RemoveListener(InvokeOnNormalQualityButtonClicked);
			_ultraQualityButton.OnClick.RemoveListener(InvokeOnUltraQualityButtonClicked);
			_highQualityButton.OnClick.RemoveListener(InvokeOnHighQualityButtonClicked);
			_lowQualityButton.OnClick.RemoveListener(InvokeOnLowQualityButtonClicked);
			_120FPSButton.OnClick.RemoveListener(InvokeOn120FPSButtonClicked);
			_30FPSButton.OnClick.RemoveListener(InvokeOn30FPSButtonClicked);
			_60FPSButton.OnClick.RemoveListener(InvokeOn60FPSButtonClicked);
			_90FPSButton.OnClick.RemoveListener(InvokeOn90FPSButtonClicked);
			_closeButton.OnClick.RemoveListener(InvokeOnCloseButtonClicked);
		}

		public void Initialize(float minSensitivity, float maxSensitivity, float currentSensitivity)
		{
			_sensitivitySlider.value = currentSensitivity;
			_sensitivitySlider.minValue = minSensitivity;
			_sensitivitySlider.maxValue = maxSensitivity;
			_sensitivityValueText.text = currentSensitivity.ToString("F1");
		}
		
		public void UpdateGraphicsQualityButtonsState(GraphicsQualityType graphicsQuality)
		{
			_normalQualityButton.UnSelect();
			_ultraQualityButton.UnSelect();
			_highQualityButton.UnSelect();
			_lowQualityButton.UnSelect();
			
			switch (graphicsQuality)
			{
				case GraphicsQualityType.Low:
					_lowQualityButton.Select();
					break;
				case GraphicsQualityType.Normal:
				default:
					_normalQualityButton.Select();
					break;
				case GraphicsQualityType.High:
					_highQualityButton.Select();
					break;
				case GraphicsQualityType.Ultra:
					_ultraQualityButton.Select();
					break;
			}
		}

		public void UpdateFPSLimitButtonsState(int fpsLimit)
		{
			_120FPSButton.UnSelect();
			_90FPSButton.UnSelect();
			_60FPSButton.UnSelect();
			_30FPSButton.UnSelect();

			switch (fpsLimit)
			{
				case 30:
					_30FPSButton.Select();
					break;
				case 60:
					_60FPSButton.Select();
					break;
				case 90:
					_90FPSButton.Select();
					break;
				case 120:
					_120FPSButton.Select();
					break;
				default:
					_60FPSButton.Select();
					break;
			}
		}
		
		private void InvokeOnSensitivitySliderValueChanged(float value)
		{
			_sensitivityValueText.text = value.ToString("F1");
			OnSensitivitySliderValueChanged?.Invoke(value);
		}

		private void InvokeOnNormalQualityButtonClicked() => OnNormalQualityButtonClicked?.Invoke();
		
		private void InvokeOnUltraQualityButtonClicked() => OnUltraQualityButtonClicked?.Invoke();
		
		private void InvokeOnHighQualityButtonClicked() => OnHighQualityButtonClicked?.Invoke();
		
		private void InvokeOnLowQualityButtonClicked() => OnLowQualityButtonClicked?.Invoke();
		
		private void InvokeOn120FPSButtonClicked() => On120FPSButtonClicked?.Invoke();
		
		private void InvokeOn30FPSButtonClicked() => On30FPSButtonClicked?.Invoke();
		
		private void InvokeOn60FPSButtonClicked() => On60FPSButtonClicked?.Invoke();
		
		private void InvokeOn90FPSButtonClicked() => On90FPSButtonClicked?.Invoke();
		
		private void InvokeOnCloseButtonClicked() => OnCloseButtonClicked?.Invoke();
	}
}