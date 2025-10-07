using UnityEngine.Animations.Rigging;
using UnityEngine;

namespace FAS.Actors.Companion
{
	public class CompanionCostume : CharacterCostume, ICompanionCostumeData
	{
		[SerializeField] private MultiRotationConstraintData _spineRig;
		[SerializeField] private MultiRotationConstraintData _headRig;
		[SerializeField] private Rig _rig;
		
		public MultiRotationConstraintData SpineRigData => _spineRig;
		public MultiRotationConstraintData HeadRigData => _headRig;
		public Rig Rig => _rig;
	}
}