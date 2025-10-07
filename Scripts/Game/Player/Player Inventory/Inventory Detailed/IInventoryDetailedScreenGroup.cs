using FAS.UI;

namespace FAS.Players
{
	public interface IInventoryDetailedScreenGroup
	{
		public void Set(UIScreen iventoryModelScreen);
		
		public void Show();
		
		public void Hide();
	}
}