using UnityEngine.Scripting.APIUpdating;
using UnityEngine;
using System;

namespace FAS
{
	[Serializable]
	[MovedFrom("FAS.Players")]
	public class CostumeMeleeWeaponPoints : CostumeWeaponPoints
	{
		[SerializeField] private MeleeWeaponType _meleeWeaponType;
		
		public MeleeWeaponType MeleeWeaponType => _meleeWeaponType;
		public override WeaponType WeaponType => WeaponType.Melee;
	}
}