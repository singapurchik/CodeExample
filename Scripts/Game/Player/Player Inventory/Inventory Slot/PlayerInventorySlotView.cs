using UnityEngine.UI;
using UnityEngine;
using System;
using FAS.UI;
using FAS.Utils;
using TMPro;

namespace FAS.Players
{
	public class PlayerInventorySlotView : CustomButton
	{
		[SerializeField] private Image _selectedImage;
		[SerializeField] private Image _itemImage;
		[SerializeField] private TextMeshProUGUI _amountText;
		
		public event Action<PlayerInventorySlotView> OnReturn;

		public void SetAmount(int amount) => _amountText.text = amount.ToString();
		
		public void HideSelectedImage() => _selectedImage.gameObject.TryDisable();

		public void ShowSelectedImage() => _selectedImage.gameObject.TryEnable();
		
		public void HideAmountText() => _amountText.gameObject.TryDisable();

		public void ShowAmountText() => _amountText.gameObject.TryEnable();
		
		public void SetIcon(Sprite sprite) => _itemImage.sprite = sprite;
		
		public void Return() => OnReturn?.Invoke(this);
	}
}