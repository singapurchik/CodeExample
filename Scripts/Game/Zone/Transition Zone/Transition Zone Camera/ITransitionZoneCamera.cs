using UnityEngine.Events;
using System.Collections;
using UnityEngine;

namespace FAS.Transitions
{
	public interface ITransitionZoneCamera
	{
		public Coroutine CurrentRoutine { get; }
		
		public UnityEvent OnExitFinished { get; }
		public UnityEvent OnEnterFinished { get; }
		public UnityEvent OnEnterStarted { get; }
		public UnityEvent OnExitStarted { get; }

		public IEnumerator Enter();
		
		public void Disable();
		
		public void Exit();
	}
}