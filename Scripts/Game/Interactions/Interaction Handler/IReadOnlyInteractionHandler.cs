using FAS.Players;
using UnityEngine;

namespace FAS
{
	public interface IReadOnlyInteractionHandler
	{
		public RightHandAnimType RightHandAnimType { get; }
		
		public Sprite InteractionIcon { get; }
		
		public Vector3 InteractableRotationAngles { get; }
		public Vector3 InteractablePosition { get; }
		public Vector3 CheckAccessPosition  { get; }
		
		public bool IsHasInteractionPoint { get; }
		public bool IsNeedToEyeCheck { get; }
		public bool IsUseInteractionAnimation { get; }
	}
}