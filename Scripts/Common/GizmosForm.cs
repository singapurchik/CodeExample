using System;
using UnityEngine;

namespace FAS
{
	public enum GizmosForm
	{
		Sphere,
		Cube,
		WireCube,
		WireSphere,
	}

	[Serializable]
	public struct GizmosFormData
	{
		public GizmosForm Form;
		public Color Color;
		public Vector3 Offset;
		[Header("Only for sphere")] public float Radius;
		[Header("Only for cube")] public Vector3 Size;
	}
}