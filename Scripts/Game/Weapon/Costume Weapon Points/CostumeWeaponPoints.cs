using UnityEngine.Scripting.APIUpdating;
using UnityEngine;
using System;

namespace FAS
{
	[Serializable]
	[MovedFrom("FAS.Players")]
	public abstract class CostumeWeaponPoints
	{
		[SerializeField] private Transform _rightHand;
		[SerializeField] private Transform _leftHand;
		[SerializeField] private Transform _spine;

		public abstract WeaponType WeaponType { get; }
		
		public Transform RightHand => _rightHand;
		public Transform LeftHand => _leftHand;
		public Transform Spine => _spine;
	}
}