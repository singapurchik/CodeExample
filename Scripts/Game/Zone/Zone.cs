using System.Collections.Generic;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace FAS
{
	public abstract class Zone<TZoneInside> : ZoneBase where TZoneInside : ZoneInside
	{
		[SerializeField] private CinemachineBlenderSettings _cameraBlends;
		[SerializeField] private UnityEvent<ZoneType> _onEnter;
		[SerializeField] private UnityEvent<ZoneType> _onExit;

		[Inject] private IReadOnlyList<IRangeWeaponTarget> _rangeWeaponTargets;
		
		public override IReadOnlyList<IRangeWeaponTarget> RangeWeaponTargets => _rangeWeaponTargets;
		
		protected TZoneInside Inside { get; private set; }
		
		protected bool IsActive;
		
		public override CinemachineBlenderSettings CameraBlends => _cameraBlends;
		
		public override UnityEvent<ZoneType> OnEnter => _onEnter;
		public override UnityEvent<ZoneType> OnExit => _onExit;

		protected virtual void Awake()
		{
			Inside = GetComponent<TZoneInside>();
		}

		public override void Hide() => Inside.Hide();

		public override void Enter()
		{
			Inside.Show();
			_onEnter?.Invoke(Type);
			IsActive = true;
		}

		public override void Exit()
		{
			Inside.Hide();
			_onExit?.Invoke(Type);
			IsActive = false;
		}
	}
}