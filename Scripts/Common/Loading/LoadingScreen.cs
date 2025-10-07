using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using System;
using FAS.UI;

namespace FAS
{
	public class LoadingScreen : UIInteractableScreen
	{
		[SerializeField] private Image _loadingSliderFill;
		[SerializeField] private float _autoMoveDuration = 0.5f;
		[SerializeField] private Ease _autoMoveEase = Ease.Linear;

		private Tween _currentAutoMoveTween;

		public event Action OnFillHasMaxValue;

#if UNITY_EDITOR
		protected override void Awake()
		{
			base.Awake();
			_autoMoveDuration = 3f;
		}	
#endif

		public void StartAutoMove()
		{
			_currentAutoMoveTween?.Kill();
			_loadingSliderFill.fillAmount = 0;
			_currentAutoMoveTween = DOVirtual.Float(0, 1, _autoMoveDuration, 
				value => _loadingSliderFill.fillAmount = value)
				.SetEase(_autoMoveEase)
				.OnComplete(InvokeOnFillHasMaxValue);
		}
		
		private void InvokeOnFillHasMaxValue()
		{
			OnFillHasMaxValue?.Invoke();
		}

	}
}