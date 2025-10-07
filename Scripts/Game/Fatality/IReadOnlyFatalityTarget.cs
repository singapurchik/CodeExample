using UnityEngine;
using System;

namespace FAS.Fatality
{
	public interface IReadOnlyFatalityTarget
	{
		public FatalityData CurrentData { get; }
		
		public Vector3 Position {get;}
		
		public bool IsReadyToFatality { get; }
		public bool IsEmpty { get; }
		
		public event Action OnReceivedFatality;
		public event Action OnPerformFatality;
		public event Action OnStartedFatality;
		public event Action OnFinishFatality;
	}
}