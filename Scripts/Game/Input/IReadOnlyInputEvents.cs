using System;

namespace FAS
{
	public interface IReadOnlyInputEvents
	{
		public event Action OnStopLookingIntoWindowButtonClicked;
		public event Action OnExamineInventoryItemButtonClicked;
		public event Action OnUnequipInventoryItemButtonClicked;
		public event Action OnEquipInventoryItemButtonClicked;
		public event Action OnUseInventoryItemButtonClicked;
		public event Action OnSetGirlCostumeButtonClicked;
		public event Action OnSetGuyCostumeButtonClicked;
		public event Action OnNextMonologueButtonClicked;
		public event Action OnFinishAimingButtonClicked;
		public event Action OnStartAimingButtonClicked;
		public event Action OnInteractButtonClicked;
		public event Action OnSettingsButtonClicked;
		public event Action OnCrouchButtonClicked;
		public event Action OnStandButtonClicked;
		public event Action OnShootButtonClicked;
		public event Action OnAnyButtonClicked;
	}
}