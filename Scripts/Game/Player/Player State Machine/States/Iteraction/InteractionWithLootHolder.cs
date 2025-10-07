using UnityEngine;
using Zenject;

namespace FAS.Players.States.Interaction
{
	public class InteractionWithLootHolder : PlayerState
	{
		[SerializeField] private float _takeItemAnimNTime = 0.35f;
		
		[Inject] private IPlayerInventoryAdd _inventory;

		private LootHolder _currentLootHolder;
		
		public override PlayerStates Key => PlayerStates.InteractionWithLootContainer;

		private bool _isWaitingForInteractAnimComplete;
		private bool _isWaitingForPickUpAnimComplete;
		
		public override bool IsPlayerControlledState => false;
		
		public void SetLootHolder(LootHolder lootHolder)
			=> _currentLootHolder = lootHolder;

		public override void Enter()
		{
			_isWaitingForInteractAnimComplete = true;
			_isWaitingForPickUpAnimComplete = false;
			InputControl.DisableMovementInput();
			UIScreensSwitcher.HideAll();
		}

		private void Looting()
		{
			if (_isWaitingForPickUpAnimComplete)
			{
				var animLayer = Animator.RightHandLayer;
				
				if (animLayer.IsActive)
				{
					if (_currentLootHolder.IsHasLoot && animLayer.CurrentAnimNTime > _takeItemAnimNTime)
						_currentLootHolder.TryHideItemModel();
				}
				else if (!animLayer.IsInTransition)
				{
					SoundEffects.PlayShowItemFirstTime();
					StateReturner.TryReturnLastControlledState();
					_inventory.TryAdd(_currentLootHolder.LootingItem, true);
				}
			}
			else
			{
				SoundEffects.PlayAddItem();
				Animator.PlayPickUpAnim();
				_isWaitingForPickUpAnimComplete = true;
			}
		}

		public override void Perform()
		{
			if (_isWaitingForInteractAnimComplete)
			{
				if (!Animator.RightHandLayer.IsActive)
					_isWaitingForInteractAnimComplete = false;
			}
			else if (!_currentLootHolder.IsAnimationPlaying)
			{
				if (_currentLootHolder.IsHasLoot)
					Looting();
				else
					StateReturner.TryReturnLastControlledState();
			}
		}
	}
}