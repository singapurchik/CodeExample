using System;

namespace FAS
{
	public interface ISettingView
	{
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

		public void Initialize(float minSensitivity, float maxSensitivity, float currentSensitivity);
		
		public void UpdateGraphicsQualityButtonsState(GraphicsQualityType graphicsQuality);

		public void UpdateFPSLimitButtonsState(int fpsLimit);
	}
}