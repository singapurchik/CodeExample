using System.Collections.Generic;
using Zenject;

namespace FAS
{
	public class PlayerWeaponHolder
	{
		[Inject] private List<RangeWeapon> _rangeWeapons;
		[Inject] private List<MeleeWeapon> _meleeWeapons;
		
		public List<RangeWeapon> RangeWeapons => _rangeWeapons;
		public List<MeleeWeapon> MeleeWeapons => _meleeWeapons;
		
		public RangeWeapon CurrentRangeWeapon { get; private set; }
		public MeleeWeapon CurrentMeleeWeapon { get; private set; }

		public void SetCurrentWeapon(RangeWeapon weapon) => CurrentRangeWeapon = weapon;
		
		public void SetCurrentWeapon(MeleeWeapon weapon) => CurrentMeleeWeapon = weapon;
	}
}