using UnityEngine.UI;
using UnityEngine;
using FAS.UI;

namespace FAS.Actors
{
	public class TargetDetectorView : UIScreen
	{
		[SerializeField] private GameObject _imageHolder;
		[SerializeField] private Image _detectorImage;
		
		private float _currentDetectorFillAmount;

		public void UpdateView(float fillAmountNormalized)
		{
			_currentDetectorFillAmount = fillAmountNormalized;
			_detectorImage.fillAmount = _currentDetectorFillAmount;
			_detectorImage.color = Color.Lerp(Color.yellow, Color.red, fillAmountNormalized);
		}
	}
}