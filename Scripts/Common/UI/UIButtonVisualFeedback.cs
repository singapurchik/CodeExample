using UnityEngine.UI;
using UnityEngine;
using VInspector;

namespace FAS.UI
{
	[RequireComponent(typeof(Image))]
	public class UIButtonVisualFeedback : MonoBehaviour
	{
		[SerializeField] private CustomButton _button;
		[SerializeField] private bool _isUseSeparateImages;
		[ShowIf(nameof(_isUseSeparateImages))]
		[SerializeField] private Sprite _inactiveSprite;
		[SerializeField] private Sprite _activeSprite;
		[HideIf(nameof(_isUseSeparateImages))]
		[SerializeField] private Color _activeColor;
		[EndIf]

		private Image _image;

		private void Awake()
		{
			_image = GetComponent<Image>();
			EnableInactiveSprite();
		}

		private void OnEnable()
		{
			_button.OnButtonUp.AddListener(EnableInactiveSprite);
			_button.OnButtonDown.AddListener(EnableActiveSprite);
		}
		
		private void OnDisable()
		{
			_button.OnButtonUp.RemoveListener(EnableInactiveSprite);
			_button.OnButtonDown.RemoveListener(EnableActiveSprite);
		}

		private void EnableInactiveSprite()
		{
			if (_isUseSeparateImages)
				_image.sprite = _inactiveSprite;
			else
				_image.color = Color.white;
		}
		
		private void EnableActiveSprite()
		{
			if (_isUseSeparateImages)
				_image.sprite = _activeSprite;
			else
				_image.color = _activeColor;
		}
	}
}