namespace FAS.Actors.Companion
{
	public class CompanionCostumeChanger : CharacterCostumeChanger<CompanionCostume, ICompanionCostumeData>,
		ICompanionCostumeProxy
	{
		public override bool TryChangeCostume(CharacterCostumeType costumeType, bool isSavingAnimatorStates = true)
			=> base.TryChangeCostume(GetCostumeType(costumeType), isSavingAnimatorStates);
		
		public void ForceChangeCostume(CharacterCostumeType costumeType, bool isSavingAnimatorStates = true)
		 => ChangeCostume(GetCostumeType(costumeType), isSavingAnimatorStates);
		
		private CharacterCostumeType GetCostumeType(CharacterCostumeType costumeType)
		{
			switch (costumeType)
			{
				default:
				case CharacterCostumeType.Girl:
					return CharacterCostumeType.Guy;
				case CharacterCostumeType.Guy:
					return CharacterCostumeType.Girl;
			}
		}
	}
}