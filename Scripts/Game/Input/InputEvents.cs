using System;

namespace FAS
{
	public class InputEvents : IReadOnlyInputEvents
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
		
		public void InvokeOnFinishAimingButtonClicked() => InvokeButtonClicked(OnFinishAimingButtonClicked);
		
		public void InvokeOnStartAimingButtonClicked() => InvokeButtonClicked(OnStartAimingButtonClicked);
		
		public void InvokeOnShootButtonClicked() => InvokeButtonClicked(OnShootButtonClicked);
		
		public void InvokeOnStopLookingIntoWindowButtonClicked()
			=> InvokeButtonClicked(OnStopLookingIntoWindowButtonClicked);
		
		public void InvokeOnUnequipInventoryItemButtonClicked()
			=> InvokeButtonClicked(OnUnequipInventoryItemButtonClicked);
		
		public void InvokeOnEquipInventoryItemButtonClicked()
			=> InvokeButtonClicked(OnEquipInventoryItemButtonClicked);
		
		public void InvokeOnExamineInventoryItemButtonClicked()
			=> InvokeButtonClicked(OnExamineInventoryItemButtonClicked);
		
		public void InvokeOnUseInventoryItemButtonClicked()
			=> InvokeButtonClicked(OnUseInventoryItemButtonClicked);
		
		public void InvokeOnSetGirlCostumeButtonClicked() => InvokeButtonClicked(OnSetGirlCostumeButtonClicked);
		
		public void InvokeOnSetGuyCostumeButtonClicked() => InvokeButtonClicked(OnSetGuyCostumeButtonClicked);
		
		public void InvokeOnNextMonologueButtonClicked() => InvokeButtonClicked(OnNextMonologueButtonClicked);

		public void InvokeOnInteractButtonClicked() => InvokeButtonClicked(OnInteractButtonClicked);

		public void InvokeOnSettingsButtonClicked() => InvokeButtonClicked(OnSettingsButtonClicked);

		public void InvokeOnCrouchButtonClicked() => InvokeButtonClicked(OnCrouchButtonClicked);

		public void InvokeOnStandButtonClicked() => InvokeButtonClicked(OnStandButtonClicked);
		
		public void InvokeOnAnyButtonClicked() => OnAnyButtonClicked?.Invoke();

		private void InvokeButtonClicked(Action action)
		{
			InvokeOnAnyButtonClicked();
			action?.Invoke();
		}
	}
}