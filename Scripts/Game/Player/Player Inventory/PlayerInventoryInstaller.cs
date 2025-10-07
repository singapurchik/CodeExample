using FAS.Players.States.Interaction;
using System.Collections.Generic;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Players
{
	public class PlayerInventoryInstaller : MonoInstaller
	{
		[SerializeField] private List<InventoryItemModelRotationInputPanel> _inputPanels = new (2);
		[SerializeField] private PlayerInventorySlotViewPool _inventorySlotViewPool;
		[SerializeField] private InventoryDetailedScreen _inventoryDetailedScreen;
		[SerializeField] private InventoryItemModelDisplayer _itemModelDisplayer;
		[SerializeField] private InteractionUsedItemDisplayer _usedItemDisplayer;
		[SerializeField] private InventoryItemModelRotator _itemModelRotator;
		[SerializeField] private InventoryQuickBarScreen _inventoryQuickBar;
		[SerializeField] private InventoryItemModelScreen _itemModelScreen;
		[SerializeField] private InteractionUsedItemScreen _usedItemScreen;
		[SerializeField] private InventoryDetailed _inventoryDetailed;
		[SerializeField] private PlayerInventory _inventory;

		private readonly RangeWeaponInventoryModels _rangeWeaponModels = new ();
		private readonly MeleeWeaponInventoryModels _meleeWeaponModels = new ();
		private readonly AmmoInventoryModels _ammoModels = new ();
		private readonly ItemInventoryModels _itemModels = new ();
		
		private readonly PlayerInventoryWeapons _inventoryWeapons = new ();
		private readonly PlayerInventoryItems _inventoryItems = new ();
		private readonly PlayerInventoryAmmo _inventoryAmmo = new ();
		
		public override void InstallBindings()
		{
			Container.Bind<IPlayerInventoryAdd>().FromInstance(_inventory)
				.WhenInjectedInto<InteractionWithLootHolder>();
			
			Container.Bind<IPlayerInventoryAdd>().FromInstance(_inventory)
				.WhenInjectedInto<PlayerWeapon>();

			Container.Bind<IInventoryDetailedUpdater>().FromInstance(_inventoryDetailed).AsSingle();
			Container.Bind<IReadOnlyPlayerInventory>().FromInstance(_inventory).AsSingle();

			Container.BindInstance(_itemModelScreen).WhenInjectedIntoInstance(_itemModelDisplayer);
			
			Container.BindInstance(_inventorySlotViewPool).WhenInjectedInto<PlayerInventoryObject>();
			Container.BindInstance(_inventoryQuickBar).WhenInjectedInto<PlayerInventoryObject>();
			Container.BindInstance(_inputPanels).WhenInjectedInto<InventoryItemModelRotator>();
			Container.BindInstance(_inventory).WhenInjectedInto<PlayerInventoryItemsUser>();
			Container.BindInstance(_inventory).WhenInjectedInto<PlayerInteractor>();

			BindInstanceToInteractionUsedItemDisplayer(_itemModelDisplayer);
			BindInstanceToInteractionUsedItemDisplayer(_itemModelRotator);
			BindInstanceToInteractionUsedItemDisplayer(_usedItemScreen);
			
			BindInstanceToInventoryDetailed(_inventoryDetailedScreen);
			BindInstanceToInventoryDetailed(_itemModelDisplayer);
			BindInstanceToInventoryDetailed(_itemModelRotator);
			
			BindInstanceToInventory(_inventoryDetailed);
			BindInstanceToInventory(_rangeWeaponModels);
			BindInstanceToInventory(_meleeWeaponModels);
			BindInstanceToInventory(_inventoryWeapons);
			BindInstanceToInventory(_inventoryItems);
			BindInstanceToInventory(_inventoryAmmo);
			BindInstanceToInventory(_ammoModels);
			BindInstanceToInventory(_itemModels);

			BindInstanceToItemDisplayer(_rangeWeaponModels);
			BindInstanceToItemDisplayer(_meleeWeaponModels);
			BindInstanceToItemDisplayer(_ammoModels);
			BindInstanceToItemDisplayer(_itemModels);
			
			BindInstanceToInventoryWeapons(_inventoryDetailed);
			BindInstanceToInventoryWeapons(_usedItemDisplayer);
			BindInstanceToInventoryWeapons(_rangeWeaponModels);
			BindInstanceToInventoryWeapons(_meleeWeaponModels);
			Container.QueueForInject(_inventoryWeapons);
			
			BindInstanceToInventoryItems(_inventoryDetailed);
			BindInstanceToInventoryItems(_usedItemDisplayer);
			BindInstanceToInventoryItems(_itemModels);
			Container.QueueForInject(_inventoryItems);

			BindInstanceToInventoryAmmo(_ammoModels);
			Container.QueueForInject(_inventoryAmmo);
		}
		
		private void BindInstanceToItemDisplayer<T>(T instance) where T : class
			=> Container.BindInstance(instance).WhenInjectedIntoInstance(_itemModelDisplayer);

		private void BindInstanceToInventoryAmmo<T>(T instance) where T : class
			=> Container.BindInstance(instance).WhenInjectedIntoInstance(_inventoryAmmo);
		
		private void BindInstanceToInventoryWeapons<T>(T instance) where T : class
			=> Container.BindInstance(instance).WhenInjectedIntoInstance(_inventoryWeapons);
		
		private void BindInstanceToInventoryItems<T>(T instance) where T : class
			=> Container.BindInstance(instance).WhenInjectedIntoInstance(_inventoryItems);

		private void BindInstanceToInteractionUsedItemDisplayer<T>(T instance) where T : class
			=> Container.BindInstance(instance).WhenInjectedIntoInstance(_usedItemDisplayer);

		private void BindInstanceToInventoryDetailed<T>(T instance) where T : class
			=> Container.BindInstance(instance).WhenInjectedIntoInstance(_inventoryDetailed);

		private void BindInstanceToInventory<T>(T instance) where T : class
			=> Container.BindInstance(instance).WhenInjectedIntoInstance(_inventory);
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_inputPanels.Clear();
			_inputPanels.AddRange(FindObjectsOfType<InventoryItemModelRotationInputPanel>(true));
			
			_inventorySlotViewPool = FindObjectOfType<PlayerInventorySlotViewPool>(true);
			_inventoryDetailedScreen = FindObjectOfType<InventoryDetailedScreen>(true);
			_itemModelDisplayer = FindObjectOfType<InventoryItemModelDisplayer>(true);
			_usedItemDisplayer = FindObjectOfType<InteractionUsedItemDisplayer>(true);
			_itemModelRotator = FindObjectOfType<InventoryItemModelRotator>(true);
			_inventoryQuickBar = FindObjectOfType<InventoryQuickBarScreen>(true);
			_itemModelScreen = FindObjectOfType<InventoryItemModelScreen>(true);
			_usedItemScreen = FindObjectOfType<InteractionUsedItemScreen>(true);
			_inventoryDetailed = FindObjectOfType<InventoryDetailed>(true);
			_inventory = FindObjectOfType<PlayerInventory>(true);
		}
#endif
	}
}