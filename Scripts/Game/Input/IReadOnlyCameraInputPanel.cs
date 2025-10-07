using UnityEngine;

namespace FAS
{
	public interface IReadOnlyCameraInputPanel
	{
		public Vector2 CurrentInputVector { get; }
        
		public bool IsInputProcess { get; }
	}
}