using UnityEngine;

namespace FAS
{
	public interface ICharacterInfo
	{
		public HumanoidBonesHolder Bones { get; }
		public Vector3 EulersAngles { get; }
		public Vector3 Position { get; }
	}
}