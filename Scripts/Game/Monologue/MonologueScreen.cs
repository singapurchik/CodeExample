using UnityEngine;
using TMPro;

namespace FAS.UI
{
	public class MonologueScreen : UIInteractableScreen, IMonologueText
	{
		[SerializeField] private TextMeshProUGUI _monologueText;
		
		public void Set(string text) => _monologueText.text = text;
	}
}