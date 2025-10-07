using System.Collections.Generic;
using FAS.Players.Animations;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Players
{
	public class PlayerWeapon : MonoBehaviour, IPlayerRangeWeapon
	{
		[SerializeField] private bool _isHasStartRangeWeapon;
		[ShowIf(nameof(_isHasStartRangeWeapon))]
		[SerializeField] private RangeWeapon _startRangeWeapon;
		[EndIf]
		[SerializeField] private bool _isHasStartMeleeWeapon;
		[ShowIf(nameof(_isHasStartMeleeWeapon))]
		[SerializeField] private MeleeWeapon _startMeleeWeapon;
		[EndIf]
		
		[Inject] private RangeWeaponTargetFinder _targetFinder;
		[Inject] private PlayerAnimEventsReceiver _animEvents;
		[Inject] private IWeaponStatesSwitcher _statesSwitcher;
		[Inject] private PlayerWeaponEquipper _weaponEquipper;
		[Inject] private IPlayerCostumeProxy _costumeProxy;
		[Inject] private IReadOnlyInputEvents _inputEvents;
		[Inject] private PlayerWeaponHolder _weaponHolder;
		[Inject] private IPlayerInventoryAdd _inventory;
		[Inject] private PlayerAnimator _animator;

		private IReadOnlyList<CostumeWeaponPoints> _weaponPoints;

		private CostumeRangeWeaponPoints _currentRangeWeaponPoints;
		private CostumeMeleeWeaponPoints _currentMeleeWeaponPoints;

		public IMeleeWeaponInfo CurrentMeleeWeapon => _weaponHolder.CurrentMeleeWeapon;
		public IRangeWeaponInfo Info => _weaponHolder.CurrentRangeWeapon;

		private void Awake()
		{
			TryCreateWeaponPoints(_costumeProxy.Data);
		}
		
		private void OnEnable()
		{
			_inputEvents.OnUnequipInventoryItemButtonClicked += OnTryUnequipFromInventory;
			_inputEvents.OnEquipInventoryItemButtonClicked += OnTryEquipFromInventory;
			_inputEvents.OnStartAimingButtonClicked += OnStartAimingButtonClicked;
			_animEvents.OnSetWeaponToRightHand += OnSetRangeWeaponToRightHand;
			_animEvents.OnSetWeaponToLeftHand += OnSetRangeWeaponToLeftHand;
			_costumeProxy.OnCostumeChanged += TryCreateWeaponPoints;
			_animEvents.OnSetWeaponToSpine += OnSetRangeWeaponToSpine;
		}
		
		private void OnDisable()
		{
			_inputEvents.OnUnequipInventoryItemButtonClicked -= OnTryUnequipFromInventory;
			_inputEvents.OnEquipInventoryItemButtonClicked -= OnTryEquipFromInventory;
			_inputEvents.OnStartAimingButtonClicked -= OnStartAimingButtonClicked;
			_animEvents.OnSetWeaponToRightHand -= OnSetRangeWeaponToRightHand;
			_animEvents.OnSetWeaponToLeftHand -= OnSetRangeWeaponToLeftHand;
			_costumeProxy.OnCostumeChanged -= TryCreateWeaponPoints;
			_animEvents.OnSetWeaponToSpine -= OnSetRangeWeaponToSpine;
		}
		
		private void Start()
		{
			if (_isHasStartRangeWeapon)
			{
				_inventory.TryAdd(_startRangeWeapon.Data, false);
				_weaponEquipper.ChangeRangeWeapon(_startRangeWeapon);
			}

			if (_isHasStartMeleeWeapon)
			{
				_inventory.TryAdd(_startMeleeWeapon.Data, false);
				_weaponEquipper.ChangeMeleeWeapon(_startMeleeWeapon);
			}
		}
		
		private void OnStartAimingButtonClicked()
		{
			_targetFinder.TrySelectClosestIfNone();
			
			if (_targetFinder.IsHasTarget)
				_statesSwitcher.SwitchToRangeWeaponReadyState();
		}
		
		private void OnSetRangeWeaponToRightHand(float duration) => _weaponEquipper.SetRangeWeaponToRightHand(duration);
		
		private void OnSetRangeWeaponToLeftHand(float duration) => _weaponEquipper.SetRangeWeaponToLeftHand(duration);
		
		private void OnSetRangeWeaponToSpine(float duration) => _weaponEquipper.SetRangeWeaponToSpine(duration);
		
		private void OnTryUnequipFromInventory() => _weaponEquipper.OnTryUnequipFromInventory();
		
		private void OnTryEquipFromInventory() => _weaponEquipper.OnTryEquipFromInventory();

		private void TryCreateWeaponPoints(IPlayerCostumeData data)
		{
			if (_weaponPoints == null && data.WeaponPoints != null && data.WeaponPoints.Count > 0)
			{
				_weaponPoints = data.WeaponPoints;
				
				foreach (var points in _weaponPoints)
				{
					if (points.WeaponType == WeaponType.Range)
					{
						var rangePoints = (CostumeRangeWeaponPoints)points;

						foreach (var rangeWeapon in _weaponHolder.RangeWeapons)
						{
							if (rangeWeapon.Data.RangeWeaponType == rangePoints.RangeWeaponType)
							{
								rangeWeapon.SetCostumePoints(rangePoints);
								break;
							}
						}
					}
					else if (points.WeaponType == WeaponType.Melee)
					{
						var meleePoints = (CostumeMeleeWeaponPoints)points;

						foreach (var meleeWeapon in _weaponHolder.MeleeWeapons)
						{
							if (meleeWeapon.Data.MeleeWeaponType == meleePoints.MeleeWeaponType)
							{
								meleeWeapon.SetCostumePoints(meleePoints);
								break;
							}
						}
					}
				}
			}
		}
		
		public void Shoot()
		{
			
		}
	}
}