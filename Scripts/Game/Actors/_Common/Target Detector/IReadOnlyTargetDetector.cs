using UnityEngine;
using System;

namespace FAS.Actors
{
	public interface IReadOnlyTargetDetector
	{
		public Vector3 TargetPosition { get; }
		
		public bool IsTargetDetected { get; }

		public event Action OnTargetDetected;
	}
}