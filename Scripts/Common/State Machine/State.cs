using UnityEngine;
using System;

namespace FAS
{
	public abstract class State<TStateEnum, TNextStateKeyEnum> : MonoBehaviour
		where TStateEnum : struct, Enum
		where TNextStateKeyEnum : struct, Enum
	{
		public abstract TStateEnum Key { get; }
		
		public TNextStateKeyEnum NextStateKey {get; protected set;}
		
		public bool IsReadyToTransit {get; protected set;}
		
		public virtual void Exit()
		{
			IsReadyToTransit = false;
		}

		protected void RequestTransition(TNextStateKeyEnum nextState)
		{
			NextStateKey = nextState;
			IsReadyToTransit = true;
		}
		
		public abstract void Enter();
		
		public virtual void Perform() { }
	}
}