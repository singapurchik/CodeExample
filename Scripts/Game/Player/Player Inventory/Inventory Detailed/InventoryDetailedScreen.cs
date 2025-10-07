using UnityEngine;
using FAS.Utils;
using Zenject;
using System;
using FAS.UI;
using TMPro;

namespace FAS.Players
{
	public class InventoryDetailedScreen : UIInteractableScreen
	{
		[SerializeField] private TextMeshProUGUI _itemNameText;
		[SerializeField] private GameObject _descriptionWindow;
		[SerializeField] private TextMeshProUGUI _descriptionText;
		[SerializeField] private CustomButton _closeButton;
		
		[Inject] private IReadOnlyInputEvents _inputEvents;
		
		public bool IsDescriptionShown => _descriptionWindow.activeSelf;
		
		public event Action OnCloseButtonClicked;
		public event Action OnDescriptionHidden;
		public event Action OnDescriptionShown;

		private void OnEnable()
		{
			_inputEvents.OnExamineInventoryItemButtonClicked += TryShowDescription;
			_closeButton.OnClick.AddListener(InvokeOnCloseButtonClicked);
		}

		private void OnDisable()
		{
			_inputEvents.OnExamineInventoryItemButtonClicked -= TryShowDescription;
			_closeButton.OnClick.RemoveListener(InvokeOnCloseButtonClicked);
		}

		public void UpdateScreen(string itemName, string description)
		{
			UpdateItemName(itemName);
			UpdateDescription(description);
			TryHideDescription();
		}
		
		private void UpdateDescription(string description) => _descriptionText.text = description;

		private void UpdateItemName(string itemName) => _itemNameText.text = itemName;

		public void TryHideDescription()
		{
			if (_descriptionWindow.TryDisable())
				OnDescriptionHidden?.Invoke();
		}

		private void TryShowDescription()
		{
			if (_descriptionWindow.TryEnable())
				OnDescriptionShown?.Invoke();
		}

		public override void Hide()
		{
			TryHideDescription();
			base.Hide();
		}
		
		private void InvokeOnCloseButtonClicked() => OnCloseButtonClicked?.Invoke();
	}
}