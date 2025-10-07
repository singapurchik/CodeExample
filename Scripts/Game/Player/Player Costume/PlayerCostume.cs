using System.Collections.Generic;
using UnityEngine;

namespace FAS.Players
{
	public class PlayerCostume : CharacterCostume, IPlayerCostumeData
	{
		[SerializeField] private PlayerVisualEffectsData _visualEffects;
		[SerializeReference] private List<CostumeWeaponPoints> _weaponPoints = new ();

		public IReadOnlyList<CostumeWeaponPoints> WeaponPoints => _weaponPoints;
		public PlayerVisualEffectsData VisualEffects => _visualEffects;
	}
}