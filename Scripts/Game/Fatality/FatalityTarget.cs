using UnityEngine;
using System;

namespace FAS.Fatality
{
	public class FatalityTarget : MonoBehaviour, IFatalityTarget
	{
		public FatalityData CurrentData { get; private set; }
		
		public Vector3 Position => transform.position;
		
		public bool IsFatalityCompleted { get; private set; }
		public bool IsReadyToFatality { get; private set; }
		public bool IsEmpty { get; private set; } = true;
		
		public event Action OnReceivedFatality;
		public event Action OnStartedFatality;
		public event Action OnPerformFatality;
		public event Action OnFinishFatality;


		public void PrepareFatality(FatalityData data)
		{
			IsEmpty = false;
			CurrentData = data;
			OnReceivedFatality?.Invoke();
		}
		
		public void SetFatalityCompletedState(bool sate) => IsFatalityCompleted = sate;

		public void SetReadyToFatality() => IsReadyToFatality = true;
		
		public void StartFatality()
		{
			OnStartedFatality?.Invoke();
		}

		public void PerformFatality() => OnPerformFatality?.Invoke();
		
		public void FinishFatality() => OnFinishFatality?.Invoke();
	}
}