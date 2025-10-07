using FAS.Actors.Companion.Animations;
using System.Collections;
using FAS.Players;
using UnityEngine;
using Zenject;
using System;

namespace FAS.Actors.Companion
{
	public class Companion : MonoBehaviour, ICompanion, IInteractableVisitable
	{
		[Inject] private CompanionCostumeChanger _costumeChanger;
		[Inject] private IPlayerCostumeProxy _playerCostumeProxy;
		[Inject] private CompanionStateMachine _stateMachine;
		[Inject] private CompanionAnimator _animator;
		[Inject] private CompanionMover _mover;

		[Inject] private IAnimatorDataSaver _animatorDataSaver;

		private Coroutine _enableFirstTimeRoutine;
		
		private bool _isInitializedRequested;
		private bool _isFirstTime = true;
		
		public HumanoidBonesHolder Bones => _costumeChanger.Data.BonesHolder;
		public ICompanionOwner Owner { get; private set; }

		public Vector3 EulersAngles => transform.eulerAngles;
		public Vector3 Position => transform.position;
		
		public bool IsInitialized { get; private set; }
		
		public event Action OnInitialized;

		private void OnEnable()
		{
			if (_isFirstTime)
			{
				_costumeChanger.TryChangeCostume(_playerCostumeProxy.Data.Type, false);
				
				if (_enableFirstTimeRoutine != null)
					StopCoroutine(_enableFirstTimeRoutine);
				
				_enableFirstTimeRoutine = StartCoroutine(EnableFirstTimeRoutine());
			}
			else if (!_costumeChanger.TryChangeCostume(_playerCostumeProxy.Data.Type, false))
			{
				_animatorDataSaver.LoadData();
			}

			_playerCostumeProxy.OnCostumeChanged += OnOwnerCostumeChanged;
		}
		
		private void OnDisable()
		{
			_playerCostumeProxy.OnCostumeChanged -= OnOwnerCostumeChanged;
		}
		
		private IEnumerator EnableFirstTimeRoutine()
		{
			_stateMachine.Initialize();
			yield return new WaitForFixedUpdate();
			yield return new WaitForFixedUpdate();
			yield return new WaitForFixedUpdate();
			_animatorDataSaver.SaveData();
			_isFirstTime = false;
		}

		private void OnOwnerCostumeChanged(IPlayerCostumeData ownerCostumeData)
		{
			_costumeChanger.TryChangeCostume(ownerCostumeData.Type);
		}

		public void TryInitialize(ICompanionOwner owner)
		{
			if (!IsInitialized)
			{
				Owner = owner;
				transform.SetParent(null);
				_stateMachine.TrySwitchStateToStandUpFromBed();
				_isInitializedRequested = true;
				OnInitialized?.Invoke();	
			}
		}
		
		public void Accept(IInteractableVisitor visitor) => visitor.Apply(this);
		
		private void Update()
		{
			if (!IsInitialized)
			{
				_mover.RequestDisable();

				if (_isInitializedRequested)
					IsInitialized = true;
			}
		}
	}
}