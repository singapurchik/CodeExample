using System.Collections.Generic;
using UnityEngine.Events;
using FAS.Players;
using UnityEngine;
using VInspector;
using FAS.Sounds;
using FAS.Items;
using System;

namespace FAS
{
	public class InteractionHandler : MonoBehaviour, IReadOnlyInteractionHandler
	{
		[SerializeField] private InteractionHandlerView _view;
		[SerializeField] private BoxCollider _collider;
		[SerializeField] private bool _isHasSuccessInteractionSound;
		[ShowIf(nameof(_isHasSuccessInteractionSound))]
		[SerializeField] private SoundEvent _successInteractionSound;
		[EndIf]
		[SerializeField] private bool _isHasFailureInteractionSound;
		[ShowIf(nameof(_isHasFailureInteractionSound))]
		[SerializeField] private SoundEvent _failureInteractionSound;
		[EndIf]
		[SerializeField] private BodyInteractionAnimType _bodyInteractionAnimType;
		[SerializeField] private bool _isUseInteractionAnimation;
		[ShowIf(nameof(_isUseInteractionAnimation))]
		[SerializeField] private RightHandAnimType _rightHandAnimType;
		[EndIf]
		[SerializeField] private bool _isHasInteractionPoint;
		[ShowIf(nameof(_isHasInteractionPoint))]
		[SerializeField] private bool _isUnParentInteractionPointOnAwake;
		[SerializeField] private Transform _interactionPoint;
		[EndIf]
		[SerializeField] private bool _isNeedToEyeCheck;
		[ShowIf(nameof(_isNeedToEyeCheck))]
		[SerializeField] private Transform _checkAccessPoint;
		[EndIf]
		[SerializeField] private bool _isNeedItemToInteract;
		[ShowIf(nameof(_isNeedItemToInteract))]
		[SerializeField] private ItemType _itemTypeToInteract;
		[SerializeField] private bool _isDestroyItemAfterUse;
		[SerializeField] private List<MonologueData> _noItemMonologues = new (0);
		[SerializeField] private float _monologueDelay;
		[SerializeField] private bool _isLoopedMonologue;
		[EndIf]

		[Space(10)]
		public UnityEvent<IInteractableVisitor> OnVisit;
		public UnityEvent OnRestore;

		private int _currentNoItemMonologueIndex;
		
		public BodyInteractionAnimType BodyInteractionAnimType => _bodyInteractionAnimType;
		public RightHandAnimType RightHandAnimType => _rightHandAnimType;
		public ItemType ItemTypeToInteract => _itemTypeToInteract;

		public SoundEvent FailureInteractionSound => _failureInteractionSound;
		public SoundEvent SuccessInteractionSound => _successInteractionSound;
		public Sprite InteractionIcon => _view.Icon;
		
		public Vector3 InteractableRotationAngles => _interactionPoint.eulerAngles;
		public Vector3 InteractablePosition => _interactionPoint.position;
		public Vector3 CheckAccessPosition => _checkAccessPoint.position;
		
		public float MonologueDelay => _monologueDelay;
		
		public bool IsHasFailureInteractionSound => _isHasFailureInteractionSound;
		public bool IsHasSuccessInteractionSound => _isHasSuccessInteractionSound;
		public bool IsUseInteractionAnimation => _isUseInteractionAnimation;
		public bool IsDestroyItemAfterUse => _isDestroyItemAfterUse;
		public bool IsHasInteractionPoint => _isHasInteractionPoint;
		public bool IsNeedItemToInteract => _isNeedItemToInteract;
		public bool IsNeedToEyeCheck => _isNeedToEyeCheck;
		
		public event Action<InteractionHandler> OnInteract;

		private void Awake()
		{
			if (_isUnParentInteractionPointOnAwake)
				_interactionPoint.SetParent(null);
			
			_view.Hide();
		}

		public void Interact(IInteractableVisitor visitor)
		{
			if (_isNeedItemToInteract)
				_isNeedItemToInteract = false;

			Disable();
			OnVisit?.Invoke(visitor);
			OnInteract?.Invoke(this);
		}

		public void Disable()
		{
			_view.Hide();
			_collider.enabled = false;
		}

		public void SetNotReadyToInteraction() => _view.Hide();

		public void SetReadyToInteraction() => _view.Show();
		
		public bool TryGetNoItemMonologue(out MonologueData data)
		{
			if (_noItemMonologues == null || _noItemMonologues.Count == 0)
			{
				data = null;
				return false;
			}
			else
			{
				var currentMonologue = _noItemMonologues[_currentNoItemMonologueIndex];
			
				_currentNoItemMonologueIndex++;

				if (_currentNoItemMonologueIndex > _noItemMonologues.Count - 1)
				{
					if (_isLoopedMonologue)
						_currentNoItemMonologueIndex = 0;
					else
						_currentNoItemMonologueIndex = _noItemMonologues.Count - 1;
				}

				data = currentMonologue;
				return true;	
			}
		}
        
		public void Restore()
		{
			_collider.enabled = true;
			OnRestore?.Invoke();
		}
	}
}