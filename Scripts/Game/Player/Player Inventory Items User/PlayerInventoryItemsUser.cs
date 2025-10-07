using System.Collections.Generic;
using UnityEngine;
using FAS.Items;
using Zenject;

namespace FAS.Players
{
	public class PlayerInventoryItemsUser : MonoBehaviour
	{
		[Inject] private List<UsableItemEffect> _usableItemEffectsList;
		[Inject] private IReadOnlyInputEvents _inputEvents;
		[Inject] private PlayerInventory _inventory;
		
		private readonly Dictionary<ItemType, UsableItemEffect> _usableItemEffects = new (10);

		private void Awake()
		{
			foreach (var usableItemEffect in _usableItemEffectsList)
				_usableItemEffects.TryAdd(usableItemEffect.Type, usableItemEffect);
		}

		private void OnEnable()
		{
			_inputEvents.OnUseInventoryItemButtonClicked += TryUseItem;
		}

		private void OnDisable()
		{
			_inputEvents.OnUseInventoryItemButtonClicked -= TryUseItem;
		}

		private void TryUseItem()
		{
			if (_inventory.TryGetItemDataFromSelectedSlot<IItemData>(out var itemType)
			    && itemType.IsCanUseFromInventory && _usableItemEffects.TryGetValue(itemType.Type, out var effect))
			{
				effect.Play();
				_inventory.Items.TryRemove(itemType.Type);
			}
		}
	}
}