using UnityEngine.UI;
using UnityEngine;

namespace FAS.UI
{
	public class InteractButton : CustomButton
	{
		[SerializeField] private Image _interactionImage;
		
		public void SetInteractionIcon(Sprite icon) => _interactionImage.sprite = icon;
	}
}