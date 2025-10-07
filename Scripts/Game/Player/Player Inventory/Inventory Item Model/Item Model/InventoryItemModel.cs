using DG.Tweening;
using UnityEngine;

namespace FAS.Players
{
	public class InventoryItemModel : MonoBehaviour, IInventoryModelRotatable
	{
		private readonly Ease _showScaleEase = Ease.OutCubic;
		private readonly Ease _hideScaleEase = Ease.InCubic;
		
		private Quaternion _defaultRotation;
		private Vector3 _defaultPosition;
		
		private Tween _currentTween;

		public void Initialize()
		{
			_defaultRotation = transform.localRotation;
			_defaultPosition = transform.localPosition;
		}

		private void OnDisable()
		{
			_currentTween?.Kill();
		}

		public void Show()
		{
			transform.SetLocalPositionAndRotation(_defaultPosition, _defaultRotation);
			gameObject.SetActive(true);
			_currentTween?.Kill();
			transform.localScale = Vector3.one;
		}
		
		public void Hide()
		{
			transform.SetLocalPositionAndRotation(_defaultPosition, _defaultRotation);
			gameObject.SetActive(false);
		}

		public void ShowWithAnim(float duration)
		{
			Show();
			transform.localScale = Vector3.zero;
			_currentTween = transform.DOScale(Vector3.one, duration)
				.SetEase(_showScaleEase);
		}

		public void HideWithAnim(float duration)
		{
			_currentTween?.Kill();
			transform.localScale = Vector3.one;
			_currentTween = transform.DOScale(Vector3.zero, duration)
				.SetEase(_hideScaleEase)
				.OnComplete(Hide);
		}

		public void Rotate(Vector2 delta)
			=> transform.localRotation = Quaternion.Euler(delta.x, delta.y, 0) * transform.localRotation;
	}
}