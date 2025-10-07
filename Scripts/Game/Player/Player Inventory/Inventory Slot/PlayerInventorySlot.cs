using FAS.Items;
using System;

namespace FAS.Players
{
	public class PlayerInventorySlot
	{
		private readonly PlayerInventorySlotView _view;

		public BaseItemData Item { get; private set; }
		
		public int ItemsInSlot { get; private set; }

		public bool IsSelected { get; private set; }
		public bool IsHasItems => ItemsInSlot > 0;
		
		public event Action<PlayerInventorySlot> OnSelected;
		
		public PlayerInventorySlot(BaseItemData item, PlayerInventorySlotView view, int itemsAmount = 1)
		{
			Item = item;
			ItemsInSlot =  itemsAmount;
			
			_view = view;
			_view.OnButtonDown.AddListener(TrySelect);

			if (item.IsStackable)
			{
				view.ShowAmountText();
				view.SetAmount(ItemsInSlot);
			}
			else
			{
				view.HideAmountText();
			}
		}
		
		public void DecreaseItems(int amount = 1)
		{
			ItemsInSlot -= amount;
			_view.SetAmount(ItemsInSlot);
		}

		public void IncreaseItems(int amount = 1)
		{
			ItemsInSlot += amount;
			_view.SetAmount(ItemsInSlot);
		}

		public void TrySelect()
		{
			if (!IsSelected)
			{
				IsSelected = true;
				_view.ShowSelectedImage();
				OnSelected?.Invoke(this);
			}
		}

		public void TryUnselect()
		{
			if (IsSelected)
			{
				_view.HideSelectedImage();
				IsSelected = false;
			}
		}
		
		public void Clear()
		{
			TryUnselect();
			_view.OnButtonDown.RemoveListener(TrySelect);
			_view.Return();
		}
	}
}