using FAS.Items;
using Zenject;
using System;

namespace FAS.Players
{
	public class PlayerInventoryObject
	{
		[Inject] protected InventoryQuickBarScreen InventoryQuickBar;
		[Inject] protected PlayerInventorySlotViewPool SlotViewPool;
		
		public event Action<PlayerInventorySlot> OnSlotCleared;
		
		protected PlayerInventorySlot CreateSlot(BaseItemData data)
		{
			var slotView = SlotViewPool.Get();
			slotView.SetIcon(data.Icon);
			InventoryQuickBar.AddSlot(slotView);
			InventoryQuickBar.RequestUpdateSlotsView();
			return new PlayerInventorySlot(data, slotView);
		}
		
		protected void InvokeOnSlotCleared(PlayerInventorySlot slot) => OnSlotCleared?.Invoke(slot);
	}
}