using UnityEngine;

namespace FAS
{
	public interface IInputVisibility
	{
		public void TryShowInventoryExamineItemButton();
		
		public void TryHideInventoryExamineItemButton();
		
		public void TryShowInventoryUnequipItemButton();
		
		public void TryHideInventoryUnequipItemButton();
		
		public void TryShowInteractButton(Sprite icon);
		
		public void TryShowInventoryEquipItemButton();
		
		public void TryHideInventoryEquipItemButton();
		
		public void TryHideUseInventoryItemButton();

		public void TryShowUseInventoryItemButton();
		
		public void TryHideInteractButton();
	}
}