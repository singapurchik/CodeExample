using UnityEngine.UI;
using UnityEngine;

namespace FAS.UI
{
	public class SelectableButton : CustomButton
	{
		[SerializeField] private Image _selectedImage;
		
		protected override void OnClickComplete()
		{
			base.OnClickComplete();
			Select();
		}

		public void UnSelect() => _selectedImage.enabled = false;
		
		public void Select() => _selectedImage.enabled = true;
	}
}