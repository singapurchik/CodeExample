using UnityEngine;
using VInspector;
using FAS.Items;
using FAS.Utils;

namespace FAS
{
	public class LootHolder : UniqueIdComponent, IInteractableVisitable
	{
		[SerializeField] private bool _isHasLootOnAwake;
		[ShowIf(nameof(_isHasLootOnAwake))]
		[SerializeField] private BaseItemData _lootingItemOnAwake;
		[SerializeField] private bool _isHasLootModel;
		[ShowIf(nameof(_isHasLootModel))]
		[SerializeField] private GameObject _lootModel;
		[EndIf]
		
		public BaseItemData LootingItem { get; private set; }
		
		public bool IsAnimationPlaying { get; protected set; }
		public bool IsHasLoot => LootingItem != null;
		public bool IsHasLootModel => _isHasLootModel;
		
		private void Awake()
		{
			if (_isHasLootOnAwake && _lootingItemOnAwake !=null)
				LootingItem = _lootingItemOnAwake;
		}
		
		public void SetLoot(ItemData item)
		{
			_lootingItemOnAwake = item;
			LootingItem = _lootingItemOnAwake;
		}

		public void TryHideItemModel()
		{
			if (_isHasLootModel)
				_lootModel.TryDisable();
		}

		protected void RemoveItem()
		{
			LootingItem = null;
			TryHideItemModel();
		}
		
		public void Accept(IInteractableVisitor visitor) => visitor.Apply(this);
	}
}