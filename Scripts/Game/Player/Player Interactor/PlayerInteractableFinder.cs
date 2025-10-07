using System.Collections.Generic;
using UnityEngine;
using FAS.Items;
using Zenject;

namespace FAS.Players
{
	[RequireComponent(typeof(SphereCollider))]
	public class PlayerInteractableFinder : MonoBehaviour, IInteractableFinderUpdater, IInteractableFinderRadius
	{
		[SerializeField] private float _findInteractionsRadius = 20f;
		[SerializeField] private float _interactRange = 3f;
		[SerializeField] private LayerMask _obstacleLayers;

		[Inject] private IReadOnlyPlayerInventory _inventory;
		[Inject] private IInputVisibility _inputVisibility;
		[Inject] private IPlayerCostumeProxy _costumeProxy;

		private HumanoidBonesHolder Bones => _costumeProxy.Data.BonesHolder;

		private enum UpdatePhase { NearbyCheck, ClosestCheck }
		private UpdatePhase _currentPhase;
		
		private SphereCollider _collider;
		
		private readonly HashSet<InteractionHandler> _nearbyInteractions = new (10);
		private readonly HashSet<InteractionHandler> _allInteractions = new (10);

		private float _requestedRadius;
		private float _currentRadius;
		private float _nextStepTime;

		private bool _isChangeRadiusRequested;
		private bool _isUpdateRequested;

		private const float UPDATE_INTERVAL = 0.125f;
		
		public InteractionHandler ClosestInteractable { get; private set; }
		
		public bool IsHasInteractable => ClosestInteractable != null;
		
		private void Awake()
		{
			_collider = GetComponent<SphereCollider>();
			_currentRadius = _findInteractionsRadius;
			_collider.radius = _currentRadius;
		}

		private void Start()
		{
			_inputVisibility.TryHideInteractButton();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out InteractionHandler interactionHandler))
			{
				TryAddToAllInteractions(interactionHandler);
				interactionHandler.SetReadyToInteraction();
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out InteractionHandler interactionHandler))
			{
				RemoveFromAllInteractions(interactionHandler);
				interactionHandler.SetNotReadyToInteraction();
			}
		}

		public void RequestChangeRadius(float radius)
		{
			_requestedRadius = radius;
			_isChangeRadiusRequested = true;
		}

		private void TryAddToAllInteractions(InteractionHandler interactionHandler)
		{
			if (_allInteractions.Add(interactionHandler))
				interactionHandler.OnInteract += RemoveFromAllInteractions;
		}
		
		private void RemoveFromAllInteractions(InteractionHandler interactionHandler)
		{
			_allInteractions.Remove(interactionHandler);
			_nearbyInteractions.Remove(interactionHandler);
			interactionHandler.OnInteract -= RemoveFromAllInteractions;

			if (ClosestInteractable == interactionHandler)
			{
				ClosestInteractable = null;
				_inputVisibility.TryHideInteractButton();
			}
		}

		public void RequestUpdate() => _isUpdateRequested = true;

		private bool IsSeeInteractionPoint(InteractionHandler interaction)
		{
			var (from, to) = GetFromTo(interaction.CheckAccessPosition);
			return !Physics.Linecast(from, to, _obstacleLayers, QueryTriggerInteraction.Collide);
		}

		private void FindNearbyInteractions()
		{
			_nearbyInteractions.Clear();
			var distance2 = _interactRange * _interactRange;

			foreach (var interaction in _allInteractions)
			{
				var (from, to) = GetFromTo(interaction.CheckAccessPosition);
				if (Vector3.SqrMagnitude(from - to) < distance2 && IsSeeInteractionPoint(interaction))
					_nearbyInteractions.Add(interaction);
			}
		}


		private void FindClosestInteractions()
		{
			ClosestInteractable = null;
			float minSqrDist = float.MaxValue;

			foreach (var interaction in _nearbyInteractions)
			{
				var (from, to) = GetFromTo(interaction.CheckAccessPosition);
				float sqrDist = Vector3.SqrMagnitude(from - to);
				if (sqrDist < minSqrDist)
				{
					ClosestInteractable = interaction;
					minSqrDist = sqrDist;
				}
			}

			if (ClosestInteractable != null)
			{
				if (ClosestInteractable.IsNeedItemToInteract
				    && _inventory.TryGetItemData(ClosestInteractable.ItemTypeToInteract, out var data))
					_inputVisibility.TryShowInteractButton(data.Icon);
				else
					_inputVisibility.TryShowInteractButton(ClosestInteractable.InteractionIcon);
			}
			else
			{
				_inputVisibility.TryHideInteractButton();
			}
		}
		
		private (Vector3 from, Vector3 to) GetFromTo(Vector3 target)
		{
			var from = Bones.LeftLeg.position;
			var to = target;
			to.y = from.y;
			return (from, to);
		}

		private void Update()
		{
			if (_isChangeRadiusRequested)
			{
				if (_currentRadius != _requestedRadius)
				{
					_currentRadius = _requestedRadius;
					_collider.radius = _currentRadius;	
				}
				_isChangeRadiusRequested = false;
			}
			else if (_currentRadius != _findInteractionsRadius)
			{
				_currentRadius = _findInteractionsRadius;
				_collider.radius = _currentRadius;
			}

			if (Time.timeSinceLevelLoad > _nextStepTime)
			{
				if (_isUpdateRequested)
				{
					switch (_currentPhase)
					{
						case UpdatePhase.NearbyCheck:
							FindNearbyInteractions();
							_currentPhase = UpdatePhase.ClosestCheck;
							break;
						case UpdatePhase.ClosestCheck:
							FindClosestInteractions();
							_currentPhase = UpdatePhase.NearbyCheck;
							break;
					}
				}
				_nextStepTime = Time.timeSinceLevelLoad + UPDATE_INTERVAL;
			}

			_isUpdateRequested = false;
			
#if UNITY_EDITOR
			if (ClosestInteractable != null)
			{
				var (from, to) = GetFromTo(ClosestInteractable.CheckAccessPosition);
				var blocked = Physics.Linecast(from, to, _obstacleLayers, QueryTriggerInteraction.Collide);
				Debug.DrawLine(from, to, blocked ? Color.red : Color.green);
			}
#endif
		}
	}
}
