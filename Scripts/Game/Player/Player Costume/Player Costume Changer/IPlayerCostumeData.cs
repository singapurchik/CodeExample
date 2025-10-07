using System.Collections.Generic;

namespace FAS.Players
{
	public interface IPlayerCostumeData : ICharacterCostumeData
	{
		public PlayerVisualEffectsData VisualEffects { get; }

		public IReadOnlyList<CostumeWeaponPoints> WeaponPoints { get; }
	}
}