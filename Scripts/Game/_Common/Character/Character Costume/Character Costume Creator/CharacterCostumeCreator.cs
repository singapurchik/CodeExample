#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace FAS
{
	public abstract class CharacterCostumeCreator : MonoBehaviour
	{
		[SerializeField] protected List<CostumePartInsideHumanoidBone> PartInsideBones = new();
		[SerializeField] protected List<Transform> PartAtRoot = new();

		protected abstract void CreateCostume();
	}
}
#endif