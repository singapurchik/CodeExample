namespace FAS.UI
{
	public class UIInteractableScreen : UIScreen
	{
		public override void Show()
		{
			CanvasGroup.blocksRaycasts = true;
			CanvasGroup.interactable = true;
			base.Show();
		}

		public override void Hide()
		{
			CanvasGroup.blocksRaycasts = false;
			CanvasGroup.interactable = false;
			base.Hide();
		}
	}
}