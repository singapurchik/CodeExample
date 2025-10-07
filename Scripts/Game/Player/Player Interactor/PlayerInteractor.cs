using FAS.Players.Animations;
using System.Collections;
using FAS.Transitions;
using FAS.Actors.Companion;
using UnityEngine;
using FAS.Sounds;
using FAS.Items;
using Zenject;
using System;
using FAS.UI;

namespace FAS.Players
{
	public class PlayerInteractor : MonoBehaviour, IInteractableVisitor, IReadOnlyPlayerInteractor
	{
		[Inject] private PlayerInteractableFinder _interactableFinder;
		[Inject] private IInteractionStatesSwitcher _statesSwitcher;
		[Inject] private IUIScreensGroupSwitcher _uiScreensSwitcher;
		[Inject] private IPlayerStateMachineInfo _stateMachineInfo;
		[Inject] private ISoundEffectsPlayer _soundEffectsPlayer;
		[Inject] private IReadOnlyInputEvents _inputEvents;
		[Inject] private ICompanionOwner _companionOwner;
		[Inject] private IDamageReceiver _damageReceiver;
		[Inject] private PlayerInventory _inventory;
		[Inject] private PlayerAnimator _animator;
		[Inject] private PlayerRotator _rotator;
		[Inject] private PlayerMover _mover;

		private InteractionHandler _currentInteractionHandler;

		private bool _isWaitingForStartAnimation;
		private bool _isWaitingForEndAnimation;
		private bool _isPrepareForInteraction;
		
		public Vector3 InteractableRotationAngles { get; private set; }
		public Vector3 InteractablePosition { get; private set; }
		
		private void OnEnable()
		{
			_inputEvents.OnInteractButtonClicked += TryInteract;
		}

		private void OnDisable()
		{
			_inputEvents.OnInteractButtonClicked -= TryInteract;
		}

		private void TryInteract()
		{
			if (_interactableFinder.IsHasInteractable)
			{
				_currentInteractionHandler = _interactableFinder.ClosestInteractable;
				
				if (_currentInteractionHandler.BodyInteractionAnimType == BodyInteractionAnimType.Crouch)
					_animator.EnableCrouchLocomotion();

				if (_currentInteractionHandler.IsHasInteractionPoint)
				{
					InteractableRotationAngles = _currentInteractionHandler.InteractableRotationAngles;
					InteractablePosition = _currentInteractionHandler.InteractablePosition;
					_statesSwitcher.SwitchToMoveToInteraction();
				}
				else
				{
					InteractablePosition = _currentInteractionHandler.InteractablePosition;
					_statesSwitcher.SwitchToRotateToInteraction();
				}
				
				_isPrepareForInteraction = true;
			}
		}
		
		private void StartInteraction()
		{
			if (_currentInteractionHandler.IsNeedItemToInteract)
			{
				if (_inventory.Items.IsHas(_currentInteractionHandler.ItemTypeToInteract))
				{
					if (_currentInteractionHandler.IsHasSuccessInteractionSound)
						_soundEffectsPlayer.PlayOneShot(_currentInteractionHandler.SuccessInteractionSound);
					
					if (_currentInteractionHandler.IsDestroyItemAfterUse)
						_inventory.Items.TryRemove(_currentInteractionHandler.ItemTypeToInteract);
					
					_uiScreensSwitcher.InteractionUsedItemScreen.AddOnHiddenListener(Interact);
				}
				else
				{
					if (_currentInteractionHandler.IsHasFailureInteractionSound)
						_soundEffectsPlayer.PlayOneShot(_currentInteractionHandler.FailureInteractionSound);
					
					StartCoroutine(NoItemForInteraction());
				}
			}
			else
			{
				if (_currentInteractionHandler.IsHasSuccessInteractionSound)
					_soundEffectsPlayer.PlayOneShot(_currentInteractionHandler.SuccessInteractionSound);
				
				Interact();
			}
		}

		private IEnumerator NoItemForInteraction()
		{
			if (_currentInteractionHandler.ItemTypeToInteract == ItemType.InsectSpray)
				_damageReceiver.TryTakeDamage(1, DamageDealerType.Flies);

			if (_currentInteractionHandler.TryGetNoItemMonologue(out var monologueData))
			{
				yield return new WaitForSeconds(_currentInteractionHandler.MonologueDelay);
				_statesSwitcher.SwitchToMonologueState(monologueData);
			}
			else
			{
				_statesSwitcher.TryReturnLastControlledState();
			}
		}

		private void Interact()
		{
			_uiScreensSwitcher.InteractionUsedItemScreen.RemoveOnHiddenListener(Interact);
			_currentInteractionHandler.Interact(this);
			_currentInteractionHandler = null;
		}

		public void Apply(ITransitionZone transition)
		{
			switch (transition.Data.TransitionType)
			{
				case TransitionType.Teleport:
					_statesSwitcher.SwitchToTeleportWithCameraEffectState(transition);
					break;
				case TransitionType.Look:
					_statesSwitcher.SwitchToInteractionWithApartmentWindow(transition);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		void IInteractableVisitor.Apply(LootHolder lootHolder)
		{
			if (lootHolder.IsHasLoot)
				_statesSwitcher.SwitchToInteractionWithLootHolder(lootHolder);
			else
				_statesSwitcher.TryReturnLastControlledState();
		}

		void IInteractableVisitor.Apply(LootContainer lootContainer)
		{
			lootContainer.Open();
			
			if (lootContainer.IsHasLoot)
			{
				_statesSwitcher.SwitchToInteractionWithLootHolder(lootContainer);
			}
			else
			{
				if (lootContainer.IsCanDropItem && lootContainer.TryDropItem())
					_statesSwitcher.SwitchToInteractionWithLootHolder(lootContainer);
				else
					_statesSwitcher.TryReturnLastControlledState();
			}
		}

		void IInteractableVisitor.Apply(DeadBody deadBody)
		{
			_statesSwitcher.SwitchToInteractionWithDeadBody(deadBody);
		}

		void IInteractableVisitor.Apply(Flies flies)
		{
			flies.SetCalmState();
			_statesSwitcher.TryReturnLastControlledState();
		}

		void IInteractableVisitor.Apply(Companion companion)
		{
			companion.TryInitialize(_companionOwner);
			_statesSwitcher.TryReturnLastControlledState();
		}

		void IInteractableVisitor.Apply(Bus bus)
		{
			if (bus.IsDoorLocked)
			{
				bus.UnlockDoor();
				_statesSwitcher.TryReturnLastControlledState();
			}
		}

		private void ProcessMoveToInteraction()
		{
			if (_currentInteractionHandler.IsUseInteractionAnimation)
			{
				_isWaitingForStartAnimation = true;
				_isWaitingForEndAnimation = true;
			}
			else
			{
				StartInteraction();
			}

			_isPrepareForInteraction = false;
		}

		private void WaitingForAnimation()
		{
			if (_isWaitingForStartAnimation)
			{
				_animator.PlayInteractionAnim(_currentInteractionHandler.RightHandAnimType);
				_isWaitingForStartAnimation = false;
			}
			else if (_isWaitingForEndAnimation
			         && _animator.RightHandLayer.IsActive
						&& _animator.RightHandLayer.IsInTransition)
			{
				StartInteraction();
				_isWaitingForEndAnimation = false;
			}
		}
		
		private void Update()
		{
			if (_isPrepareForInteraction && _stateMachineInfo.CurrentPrepareInteractionState.IsReadyToInteract)
				ProcessMoveToInteraction();

			if (_isWaitingForStartAnimation || _isWaitingForEndAnimation)
				WaitingForAnimation();
		}
	}
}