using UnityEngine.Rendering.Universal;
using UnityEngine;
using Zenject;
using System;
using FAS.UI;

namespace FAS
{
	public enum GraphicsQualityType
	{
		Low,
		Normal,
		High,
		Ultra
	}
	
	public class Settings : MonoBehaviour
	{
		[SerializeField] private UniversalRenderPipelineAsset _lowQualityAsset;
		[SerializeField] private UniversalRenderPipelineAsset _normalQualityAsset;
		[SerializeField] private UniversalRenderPipelineAsset _highQualityAsset;
		[SerializeField] private UniversalRenderPipelineAsset _ultraQualityAsset;

		[Inject] private IReadOnlyInputEvents _inputEvents;
		[Inject] private ISettingsScreenGroup _screen;
		[Inject] private ISettingView _view;
		
		private int _currentFPSLimit;
		
		public static GraphicsQualityType CurrentGraphicsQuality { get; private set; }
		
		public static float CurrentCameraSensitivity { get; private set; }
		
		private const int FPS_120 = 120;
		private const int FPS_90 = 90;
		private const int FPS_60 = 60;
		private const int FPS_30 = 30;
		
		private const float DEFAULT_CAMERA_SENSITIVITY = 10f;
		private const float MIN_CAMERA_SENSITIVITY = 1f;
		private const float MAX_CAMERA_SENSITIVITY = 15f;
		
		public static event Action<GraphicsQualityType> OnGraphicsQualityChanged;

		private void Awake()
		{
			CurrentCameraSensitivity = DEFAULT_CAMERA_SENSITIVITY;
			_view.Initialize(MIN_CAMERA_SENSITIVITY, MAX_CAMERA_SENSITIVITY, CurrentCameraSensitivity);
			
#if UNITY_EDITOR
			SetLowQuality();
			Set60FPS();
#elif UNITY_ANDROID
			SetNormalQuality();
			Set30FPS();
#elif UNITY_IOS
			SetHighQuality();
			Set60FPS();
#endif
		}

		private void OnEnable()
		{
			_view.OnSensitivitySliderValueChanged += SetSensitivity;
			_view.OnNormalQualityButtonClicked += SetNormalQuality;
			_inputEvents.OnSettingsButtonClicked += _screen.Open;
			_view.OnUltraQualityButtonClicked += SetUltraQuality;
			_view.OnHighQualityButtonClicked += SetHighQuality;
			_view.OnLowQualityButtonClicked += SetLowQuality;
			_view.OnCloseButtonClicked += _screen.Close;
			_view.On120FPSButtonClicked += Set120FPS;
			_view.On90FPSButtonClicked += Set90FPS;
			_view.On60FPSButtonClicked += Set60FPS;
			_view.On30FPSButtonClicked += Set30FPS;
		}

		private void OnDisable()
		{
			_view.OnSensitivitySliderValueChanged -= SetSensitivity;
			_view.OnNormalQualityButtonClicked -= SetNormalQuality;
			_inputEvents.OnSettingsButtonClicked -= _screen.Open;
			_view.OnUltraQualityButtonClicked -= SetUltraQuality;
			_view.OnHighQualityButtonClicked -= SetHighQuality;
			_view.OnLowQualityButtonClicked -= SetLowQuality;
			_view.OnCloseButtonClicked -= _screen.Close;
			_view.On120FPSButtonClicked -= Set120FPS;
			_view.On90FPSButtonClicked -= Set90FPS;
			_view.On60FPSButtonClicked -= Set60FPS;
			_view.On30FPSButtonClicked -= Set30FPS;
		}
		
		private void SetSensitivity(float value) => CurrentCameraSensitivity = value;

		private void SetLowQuality()
			=> ChangeGraphicsQuality(_lowQualityAsset, GraphicsQualityType.Low);
		
		private void SetNormalQuality()
			=> ChangeGraphicsQuality(_normalQualityAsset, GraphicsQualityType.Normal);
		
		private void SetHighQuality()
			=> ChangeGraphicsQuality(_highQualityAsset, GraphicsQualityType.High);

		
		private void SetUltraQuality()
			=> ChangeGraphicsQuality(_ultraQualityAsset, GraphicsQualityType.Ultra);


		private void ChangeGraphicsQuality(UniversalRenderPipelineAsset qualityAsset, GraphicsQualityType qualityType)
		{
			QualitySettings.SetQualityLevel(GetQualityIndexByAssetName(qualityAsset.name), true);
			CurrentGraphicsQuality = qualityType;
			_view.UpdateGraphicsQualityButtonsState(CurrentGraphicsQuality);
			OnGraphicsQualityChanged?.Invoke(CurrentGraphicsQuality);
		}
		
		private int GetQualityIndexByAssetName(string assetName)
		{
			for (int i = 0; i < QualitySettings.names.Length; i++)
				if (QualitySettings.names[i].ToLower().Contains(assetName.ToLower()))
					return i;
			
			return -1;
		}
		
		private void Set120FPS() => SetFPSLimit(FPS_120);
		
		private void Set90FPS() => SetFPSLimit(FPS_90);
		
		private void Set60FPS() => SetFPSLimit(FPS_60);
		
		private void Set30FPS() => SetFPSLimit(FPS_30);
		
		private void SetFPSLimit(int fps)
		{
			_currentFPSLimit = fps;
			Application.targetFrameRate = _currentFPSLimit;
			_view.UpdateFPSLimitButtonsState(_currentFPSLimit);
		}
	}
}