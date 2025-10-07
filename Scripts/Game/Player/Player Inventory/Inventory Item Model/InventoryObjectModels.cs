using Object = UnityEngine.Object;
using System.Collections.Generic;
using UnityEngine;
using FAS.Items;

namespace FAS.Players
{
	public sealed class AmmoInventoryModels
		: InventoryObjectModels<AmmoType> {}
	
	public sealed class RangeWeaponInventoryModels
		: InventoryObjectModels<RangeWeaponType> {}

	public sealed class MeleeWeaponInventoryModels
		: InventoryObjectModels<MeleeWeaponType> {}
	
	public sealed class ItemInventoryModels
		: InventoryObjectModels<ItemType> {}
	
	public abstract class InventoryObjectModels<TKey>
	{
		private Transform _modelsHolder;
		
		private readonly Dictionary<TKey, InventoryItemModel> _dictionary = new(10);
		
		public void Initialize(Transform modelsHolder) => _modelsHolder = modelsHolder;

		public void TryCreateModel(InventoryItemModel prefab, TKey key)
		{
			if (!_dictionary.ContainsKey(key))
			{
				var instance = Object.Instantiate(prefab, _modelsHolder);
				instance.Initialize();
				instance.Hide();
				_dictionary.Add(key, instance);
			}
		}

		public bool TryGetModel(TKey key, out InventoryItemModel model) => _dictionary.TryGetValue(key, out model);
	}
}