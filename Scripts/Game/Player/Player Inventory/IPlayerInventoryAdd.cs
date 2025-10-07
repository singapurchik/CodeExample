using FAS.Items;

namespace FAS.Players
{
	public interface IPlayerInventoryAdd
	{
		public void TryAdd(BaseItemData itemData, bool isAutoSelect);
	}
}