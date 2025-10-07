#if UNITY_EDITOR
using UnityEngine.Animations.Rigging;
using UnityEngine;

namespace FAS
{
	public class CompanionCostumeCreator : CharacterCostumeCreator
	{
		[SerializeField] private MultiRotationConstraint _spineRotationConstraint;
		[SerializeField] private MultiRotationConstraint _headRotationConstraint;

		protected override void CreateCostume()
		{
			var bonesHolder = GetComponentInChildren<HumanoidBonesHolder>();
			
			var spineData = _spineRotationConstraint.data;
			spineData.constrainedObject = bonesHolder.Spine2;
			_spineRotationConstraint.data = spineData;
			
			var headData = _headRotationConstraint.data;
			headData.constrainedObject = bonesHolder.Head;
			_headRotationConstraint.data = headData;
		}
	}
}
#endif