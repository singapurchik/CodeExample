using UnityEngine;
using System;

namespace FAS
{
	[Serializable]
	public struct CostumePartInsideHumanoidBone
	{
		public string Name;
		public Transform Part;
		public HumanoidBoneType TargetBone;
	}
}