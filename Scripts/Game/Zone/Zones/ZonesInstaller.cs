using System.Collections.Generic;
using FAS.Actors.Emenies;
using FAS.Players.States;
using FAS.Apartments;
using FAS.Corridor;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS
{
	public class ZonesInstaller : MonoInstaller
	{
		[SerializeField] private ApartmentZone _apartmentZone;
		[SerializeField] private CorridorZone _corridorZone;
		[SerializeField] private StreetZone _streetZone;
		[SerializeField] private Zones _zones;
		[SerializeField] private List<Damageable> _damageableEntities = new ();
		[SerializeField] private List<Enemy> _enemies = new ();
		
		public override void InstallBindings()
		{
			BindZonesTargets();
			
			var zones = new List<ZoneBase>
			{
				_apartmentZone,
				_corridorZone,
				_streetZone
			};
			Container.BindInstance(zones).WhenInjectedInto<Zones>();
			
			Container.Bind<IZonesHolder>().FromInstance(_zones).WhenInjectedInto<TransitionZoneState>();
			Container.Bind<IZonesHolderInfo>().FromInstance(_zones).AsSingle();
		}

		private void BindZonesTargets()
		{
			var apartmentTargets = new List<IRangeWeaponTarget>(16);
			var corridorTargets = new List<IRangeWeaponTarget>(16);
			var streetTargets = new List<IRangeWeaponTarget>(16);

			// for (int i = 0; i < _enemies.Count; i++)
			// {
			// 	var enemy = _enemies[i];
			// 	switch (enemy.ZoneType)
			// 	{
			// 		case ZoneType.Apartment:
			// 			apartmentTargets.Add(enemy);
			// 			break;
			// 		case ZoneType.Corridor:
			// 			corridorTargets.Add(enemy);
			// 			break;
			// 		case ZoneType.Street:
			// 			streetTargets.Add(enemy);
			// 			break;
			// 	}
			// }
			
			for (int i = 0; i < _damageableEntities.Count; i++)
			{
				var entity = _damageableEntities[i];
				switch (entity.ZoneType)
				{
					case ZoneType.Apartment:
						apartmentTargets.Add(entity);
						break;
					case ZoneType.Corridor:
						corridorTargets.Add(entity);
						break;
					case ZoneType.Street:
						streetTargets.Add(entity);
						break;
				}
			}

			Container.Bind<IReadOnlyList<IRangeWeaponTarget>>()
				.FromInstance(apartmentTargets).WhenInjectedIntoInstance(_apartmentZone);
			Container.Bind<IReadOnlyList<IRangeWeaponTarget>>()
				.FromInstance(corridorTargets).WhenInjectedIntoInstance(_corridorZone);
			Container.Bind<IReadOnlyList<IRangeWeaponTarget>>()
				.FromInstance(streetTargets).WhenInjectedIntoInstance(_streetZone);
		}
		
#if UNITY_EDITOR
		
		[Button]
		private void FindDependencies()
		{
			_enemies.Clear();
			_enemies.AddRange(FindObjectsOfType<Enemy>());
			
			_damageableEntities.Clear();
			_damageableEntities.AddRange(FindObjectsOfType<Damageable>());
			
			_apartmentZone = FindObjectOfType<ApartmentZone>(true);
			_corridorZone = FindObjectOfType<CorridorZone>(true);
			_streetZone = FindObjectOfType<StreetZone>(true);
			_zones = FindObjectOfType<Zones>(true);
		}
#endif
	}
}