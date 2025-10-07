namespace FAS
{
	public interface ICharacterCostumeData
	{
		public HumanoidBonesHolder BonesHolder { get; }
		public IClothesWetness ClothesWetness { get; }
		public AnimatorData AnimatorData { get; }
		public IFakeShadow FakeShadow { get; }
		
		public CharacterCostumeType Type { get; }
	}
}