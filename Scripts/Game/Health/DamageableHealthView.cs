using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using FAS.Utils;
using FAS.UI;
using TMPro;

namespace FAS
{
	[RequireComponent(typeof(LookAtCamera))]
	public class DamageableHealthView : UIScreen
	{
		[SerializeField] private Image _skullImage;
		[SerializeField] private Image _sliderFill;
		[SerializeField] private TextMeshProUGUI _healthText;

		[Header("Fill Blink (alpha)")]
		[SerializeField, Range(0f, 1f)] private float _minAlpha = 0.8f;
		[SerializeField, Min(0f)] private float _duration = 0.4f;

		private LookAtCamera _lookAtCamera;
		private Tweener _alphaBlinkTween;

		protected override void Awake()
		{
			base.Awake();
			_lookAtCamera = GetComponent<LookAtCamera>();
			CreateBlinkTween();
			Hide();
		}
		
		private void OnDestroy()
		{
			if (_alphaBlinkTween != null && _alphaBlinkTween.IsActive())
				_alphaBlinkTween.Kill();
		}

		protected override void ShowComplete()
		{
			base.ShowComplete();
			_lookAtCamera.enabled = true;
			_lookAtCamera.OnUpdate();
			TryPlayBlinkAnim();
		}

		protected override void HideComplete()
		{
			base.HideComplete();
			_skullImage.gameObject.TryDisable();
			_lookAtCamera.enabled = false;
			StopBlinkAnim();
		}

		private void CreateBlinkTween()
		{
			SetFillAlpha(1f);

			_alphaBlinkTween = _sliderFill
				.DOFade(_minAlpha, _duration)
				.SetLoops(-1, LoopType.Yoyo)
				.SetEase(Ease.InOutSine)
				.SetAutoKill(false)
				.SetRecyclable(true)
				.Pause();
		}

		private void TryPlayBlinkAnim()
		{
			if (!_alphaBlinkTween.IsPlaying())
				_alphaBlinkTween.Play();
		}

		private void StopBlinkAnim()
		{
			_alphaBlinkTween.Rewind();
			_alphaBlinkTween.Pause();
			SetFillAlpha(1f);
		}

		private void SetFillAlpha(float alphaNormalized)
		{
			var color = _sliderFill.color;
			color.a = alphaNormalized;
			_sliderFill.color = color;
		}

		public void UpdateHealthText(float current, float max) => _healthText.SetText("{0}/{1}", current, max);
		
		public void UpdateFill(float amount) => _sliderFill.fillAmount = amount;
		
		public void HideSkullImage() => _skullImage.gameObject.TryDisable();
		
		public void ShowSkullImage() => _skullImage.gameObject.TryEnable();
	}
}
