using TMPro;
using UnityEngine;

namespace FAS.UI
{
	public class MonologueButton : CustomButton, IMonologueButtonView
	{
		[SerializeField] private TextMeshProUGUI _text;
		
		private const string NEXT_BUTTON_TEXT = "NEXT";
		private const string END_BUTTON_TEXT = "END";
		
		public void ShowNext() => _text.text = NEXT_BUTTON_TEXT;

		public void ShowEnd() => _text.text = END_BUTTON_TEXT;
	}
}