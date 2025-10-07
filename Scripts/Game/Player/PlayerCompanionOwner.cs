using UnityEngine;

namespace FAS.Players
{
	public class PlayerCompanionOwner : MonoBehaviour, ICompanionOwner
	{
		[SerializeField] private Transform _companionTeleportPoint;
		
		public Quaternion TeleportRotation => _companionTeleportPoint.rotation;
		
		public Vector3 TeleportPosition => _companionTeleportPoint.position;
		public Vector3 FollowPosition => transform.position;
	}
}