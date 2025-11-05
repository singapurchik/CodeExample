using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace Shop
{
	public class StatsView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _text;
		[SerializeField] private Button _plusButton;

		private string _label;
		
		public event Action<StatsView> OnPlusButtonClicked;
		
		public event Action<StatsView> OnRemoved;

		private void OnEnable()
		{
			_plusButton.onClick.AddListener(InvokeOnPlusButtonClicked);
		}

		private void OnDisable()
		{
			_plusButton.onClick.RemoveListener(InvokeOnPlusButtonClicked);
		}

		public void SetLabel(string label) => _label = label + ": ";

		public void SetValue(string value) => _text.text = _label + value;
		
		private void InvokeOnPlusButtonClicked() => OnPlusButtonClicked?.Invoke(this);
	}
}