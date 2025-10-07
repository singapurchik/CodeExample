using UnityEngine.Scripting.APIUpdating;
using UnityEngine;
using System;

namespace FAS
{
	[Serializable]
	[MovedFrom("FAS.Players")]
	public class CostumeRangeWeaponPoints : CostumeWeaponPoints
	{
		[SerializeField] private RangeWeaponType _rangeWeaponType;
		
		public RangeWeaponType RangeWeaponType => _rangeWeaponType;
		public override WeaponType WeaponType => WeaponType.Range;
	}
}