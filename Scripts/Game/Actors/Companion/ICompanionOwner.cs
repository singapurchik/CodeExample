using UnityEngine;

namespace FAS
{
	public interface ICompanionOwner
	{
		public Vector3 TeleportPosition { get; }
		public Vector3 FollowPosition { get; }
		
		public Quaternion TeleportRotation { get; }
	}
}