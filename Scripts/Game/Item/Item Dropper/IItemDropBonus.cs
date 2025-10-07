namespace FAS.Items
{
	public interface IItemDropBonus
	{
		void SetAdditiveBonus(ItemType itemType, float bonus);
		void SetMultiplicativeBonus(ItemType itemType, float multiplier);
		void ClearBonuses(ItemType itemType);
	}
}