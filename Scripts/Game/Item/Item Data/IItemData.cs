using UnityEngine;
using FAS.Players;
using FAS.Sounds;

namespace FAS.Items
{
	public interface IItemData
	{
		public InventoryItemModel InventoryModel { get; }
		public SoundEvent InteractionUseSound { get; }
		public ItemType Type { get; }
		public Sprite Icon { get; }
		
		public string BuildedInteractionUseText { get; }
		public string Description { get; }
		public string Name { get; }
		
		public bool IsCanEquipFromInventory { get; }
		public bool IsCanUseFromInventory { get; }
		public bool IsUsingInInteraction { get; }
		public bool IsStackable { get; }
		public bool IsEquipped { get;}
	}
}