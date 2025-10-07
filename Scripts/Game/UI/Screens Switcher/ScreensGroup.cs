using System.Collections.Generic;

namespace FAS.UI
{
	public class ScreensGroup
	{
		private readonly HashSet<UIScreen> _screens;

		public ScreensGroup(HashSet<UIScreen> screens)
		{
			_screens = screens;
		}

		public void Show()
		{
			foreach (var screen in _screens)
				screen.Show();
		}

		public void Hide()
		{
			foreach (var screen in _screens)
				screen.Hide();
		}
	}
}