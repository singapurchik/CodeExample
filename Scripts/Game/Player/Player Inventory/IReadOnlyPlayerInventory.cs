using FAS.Items;

namespace FAS.Players
{
	public interface IReadOnlyPlayerInventory
	{
		public bool TryGetItemDataFromSelectedSlot<TInterface>(out TInterface data)
			where TInterface : class, IItemData;

		public bool TryGetItemData(ItemType type, out IItemData data);
	}
}