using UnityEngine;
using VInspector;

namespace FAS
{
	public class CharacterCostume : MonoBehaviour, ICharacterCostumeData
	{
		[SerializeField] private CharacterCostumeType _type;
		[SerializeField] private AnimatorData _animatorData;
		[SerializeField] private CharacterShadowByZone _fakeShadow;
		[SerializeField] private HumanoidBonesHolder _bonesHolder;
		[SerializeField] private ClothesWetness _clothesWetness;

		public IClothesWetness ClothesWetness => _clothesWetness;
		public HumanoidBonesHolder BonesHolder => _bonesHolder;
		public AnimatorData AnimatorData => _animatorData;
		public IFakeShadow FakeShadow => _fakeShadow;
		public CharacterCostumeType Type => _type;
	}
}