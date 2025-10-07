using UnityEngine;

namespace FAS.Players
{
	public interface IReadOnlyPlayerInteractor
	{
		public Vector3 InteractableRotationAngles { get;}
		
		public Vector3 InteractablePosition { get; }
	}
}