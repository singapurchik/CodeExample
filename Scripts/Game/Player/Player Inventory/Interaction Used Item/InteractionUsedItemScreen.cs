using UnityEngine;
using System;
using FAS.UI;
using TMPro;

namespace FAS.Players
{
	public class InteractionUsedItemScreen : UIInteractableScreen
	{
		[SerializeField] private TextMeshProUGUI _nameText;
		[SerializeField] private TextMeshProUGUI _usedText;
		[SerializeField] private CustomButton _closeButton;
		
		public event Action OnCloseButtonClicked;

		private void OnEnable()
		{
			_closeButton.OnClick.AddListener(InvokeCloseButtonClicked);
		}

		private void OnDisable()
		{
			_closeButton.OnClick.RemoveListener(InvokeCloseButtonClicked);
		}

		public void UpdateScreen(string usedItemName, string usedItemText)
		{
			_nameText.text = usedItemName;
			_usedText.text = usedItemText;
		}

		private void InvokeCloseButtonClicked() => OnCloseButtonClicked?.Invoke();
	}
}