using UnityEngine;

namespace FAS
{
	public interface ICinemachineCameraTarget : ICharacterInfo
	{
		public Transform PlayerTransform { get; }
	}
}