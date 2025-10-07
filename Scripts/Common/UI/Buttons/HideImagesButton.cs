using UnityEngine.InputSystem.OnScreen;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace FAS.UI
{
	[RequireComponent(typeof(OnScreenStick))]
	public sealed class HideImagesButton : CustomButton
	{
		[SerializeField] private Image[] _images;
		[SerializeField] private bool _isHideOnPressed = true;

		private void OnDisable()
		{
			SetDefaultState();
		}

		protected override void ButtonDown(PointerEventData eventData)
		{
			SetHoldState();
			base.ButtonDown(eventData);
		}

		protected override void ButtonUp(PointerEventData eventData)
		{
			SetDefaultState();
			base.ButtonUp(eventData);
		}

		private void SetHoldState()
		{
			foreach (var image in _images)
				image.enabled = !_isHideOnPressed;
		}

		private void SetDefaultState()
		{
			foreach (var image in _images)
				image.enabled = _isHideOnPressed;
		}
	}
}