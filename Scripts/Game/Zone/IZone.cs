using System.Collections.Generic;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine;

namespace FAS
{
	public interface IZone
	{
		public bool IsReturnable { get; }
		
		public IReadOnlyList<IRangeWeaponTarget> RangeWeaponTargets { get; }
		public CinemachineBlenderSettings CameraBlends { get; }
		public Transform EnterPoint { get; }
		public ZoneType Type { get; }
		
		public UnityEvent<ZoneType> OnEnter { get; }
		public UnityEvent<ZoneType> OnExit { get; }
		
		public void Enter();
		
		public void Exit();
	}
}