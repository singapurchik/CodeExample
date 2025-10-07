using FAS.Players;
using FAS.Sounds;
using UnityEngine;

namespace FAS.Items
{
	public abstract class BaseItemData : ScriptableObject, IItemData
	{
		[SerializeField] private string _name;
		[SerializeField] private Sprite _icon;
		[SerializeField, TextArea] private string _description;
		[SerializeField] private InventoryItemModel _inventoryModel;
		
		public InventoryItemModel InventoryModel => _inventoryModel;
		public abstract SoundEvent InteractionUseSound { get; }
		public abstract ItemType Type { get; }
		public Sprite Icon => _icon;
		
		public abstract string BuildedInteractionUseText { get; }
		public string Description => _description;
		public string Name => _name;
		
		public abstract bool IsCanEquipFromInventory { get; }
		public abstract bool IsCanUseFromInventory { get; }
		public abstract bool IsUsingInInteraction { get; }
		public bool IsEquipped { get; protected set; }
		public abstract bool IsStackable { get; }
		
		public abstract bool TryResolveModel(ItemInventoryModels items, RangeWeaponInventoryModels range,
			MeleeWeaponInventoryModels melee, AmmoInventoryModels ammo, out InventoryItemModel model);
		
		public abstract bool TryAddTo(PlayerInventoryWeapons weapons, PlayerInventoryItems items,
			PlayerInventoryAmmo ammo, out PlayerInventorySlot slot);
		
		public abstract bool TryRemoveFrom(PlayerInventoryWeapons weapons,
			PlayerInventoryItems items, PlayerInventoryAmmo ammo);
	}
}