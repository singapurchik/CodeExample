using System.Collections.Generic;
using FAS.Players.States;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Players
{
	public class PlayerWeaponInstaller : MonoInstaller
	{
	    [SerializeField] private RangeWeaponTargetFinder _rangeWeaponTargetFinder;
		[SerializeField] private PlayerWeapon _weapon;
		[SerializeField] private List<RangeWeapon> _rangeWeapons = new ();
		[SerializeField] private List<MeleeWeapon> _meleeWeapons = new ();
		
		private readonly PlayerWeaponEquipper _equipper = new ();
		private readonly PlayerWeaponHolder _holder = new ();
		
		public override void InstallBindings()
		{
			Container.BindInstance(_rangeWeaponTargetFinder).WhenInjectedInto<RangeWeaponState>();
			Container.BindInstance(_rangeWeaponTargetFinder).WhenInjectedIntoInstance(_weapon);
			
			Container.BindInstance(_rangeWeapons).WhenInjectedIntoInstance(_holder);
			Container.BindInstance(_meleeWeapons).WhenInjectedIntoInstance(_holder);
			
			Container.BindInstance(_equipper).AsSingle();
			Container.BindInstance(_holder).AsSingle();
			
			Container.QueueForInject(_equipper);
			Container.QueueForInject(_holder);
		}
		
#if UNITY_EDITOR
		[Button]
		private void FindWeapons()
		{
			_meleeWeapons.Clear();
			_meleeWeapons.AddRange(GetComponentsInChildren<MeleeWeapon>(true));
			
			_rangeWeapons.Clear();
			_rangeWeapons.AddRange(GetComponentsInChildren<RangeWeapon>(true));
			
			_rangeWeaponTargetFinder = GetComponentInChildren<RangeWeaponTargetFinder>(true);
			_weapon = GetComponentInChildren<PlayerWeapon>(true);
		}
#endif
	}
}