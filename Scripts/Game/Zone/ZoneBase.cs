using System.Collections.Generic;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine;

namespace FAS
{
	public abstract class ZoneBase : MonoBehaviour, IZone
	{
		public abstract IReadOnlyList<IRangeWeaponTarget> RangeWeaponTargets { get; }
		public abstract CinemachineBlenderSettings CameraBlends { get; }
		public abstract Transform EnterPoint { get; }
		public abstract ZoneType Type { get; }
		
		public abstract bool IsReturnable { get; }
		
		public abstract UnityEvent<ZoneType> OnEnter { get; }
		public abstract UnityEvent<ZoneType> OnExit { get; }

		public abstract void Hide();
		
		public abstract void Enter();

		public abstract void Exit();
	}
}