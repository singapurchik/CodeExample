using UnityEngine.Animations.Rigging;

namespace FAS.Actors.Companion
{
	public interface ICompanionCostumeData : ICharacterCostumeData
	{
		public MultiRotationConstraintData SpineRigData { get; }
		public MultiRotationConstraintData HeadRigData { get; }
		public Rig Rig { get; }
	}
}