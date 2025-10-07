using UnityEngine;
using FAS.Items;
using Zenject;

namespace FAS.Players
{
	public class InteractionUsedItemDisplayer : MonoBehaviour
	{
		[Inject] private IInteractionUsedItemScreenGroup _interactionUsedItemScreenGroup;
		[Inject] private InventoryItemModelDisplayer _itemModelDisplayer;
		[Inject] private InventoryItemModelRotator _itemModelRotator;
		[Inject] private InteractionUsedItemScreen _screen;

		private void OnEnable()
		{
			_screen.OnCloseButtonClicked += TryHideView;
		}

		private void OnDisable()
		{
			_screen.OnCloseButtonClicked -= TryHideView;
		}

		public void ShowUsedItem(IItemData itemData, InventoryItemModel model)
		{
			_screen.UpdateScreen(itemData.Name, itemData.BuildedInteractionUseText);
			_interactionUsedItemScreenGroup.Show();
			_itemModelDisplayer.Show(model);
			_itemModelRotator.SetCurrentRotatable(_itemModelDisplayer.DisplayingItemModel);
		}

		private void TryHideView()
		{
			if (_screen.IsShown && !_screen.IsProcessHide)
			{
				_itemModelRotator.StopRotating();
				_itemModelDisplayer.Hide();
				_interactionUsedItemScreenGroup.Hide();
			}
		}
	}
}