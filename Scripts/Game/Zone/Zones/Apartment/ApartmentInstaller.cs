using FAS.Apartments.Inside.Scenarios;
using System.Collections.Generic;
using FAS.Players.States.Zone;
using FAS.Apartments.Outside;
using FAS.Apartments.Inside;
using FAS.Players.States;
using FAS.Actors.Emenies;
using FAS.Players;
using UnityEngine;
using VInspector;
using Zenject;

namespace FAS.Apartments
{
	public class ApartmentInstaller : MonoInstaller
	{
		[SerializeField] private ApartmentInsideScenarioPlayer _scenarioPlayer;
		[SerializeField] private PlayerInteractableFinder _interactableFinder;
		[SerializeField] private ApartmentCamerasSwitcher _camerasSwitcher;
		[SerializeField] private ApartmentZone _apartment;
		[SerializeField] private ApartmentInside _inside;
		[SerializeField] private Jumpscare _jumpscare;
		[SerializeField] private List<ApartmentOutside> _apartmentOutsides = new ();
		
		public override void InstallBindings()
		{
			Container.Bind<IApartmentWindow>().FromInstance(_inside).WhenInjectedInto<LookTransition>();
			Container.Bind<IApartmentWindow>().FromInstance(_inside).WhenInjectedInto<LookAtZone>();
			Container.Bind<IReadOnlyApartment>().FromInstance(_apartment).AsSingle();
			Container.BindInstance(_jumpscare).WhenInjectedInto<ShowingFace>();
			BindApartmentInside();
			BindApartment();
		}

		private void BindApartmentInside()
		{
			BindInstanceToApartmentInside(_camerasSwitcher);
			BindInstanceToApartmentInside(_scenarioPlayer);
			BindInstanceToApartmentInside(_jumpscare);
		}

		private void BindApartment()
		{
			Container.Bind<IInteractableFinderRadius>().FromInstance(_interactableFinder).WhenInjectedInto<ZoneBase>();
			BindInstanceToApartment(_apartmentOutsides);
			BindInstanceToApartment(_inside);
		}

		private void BindInstanceToApartmentInside<T>(T instance) where T : class
			=> Container.BindInstance(instance).WhenInjectedIntoInstance(_inside);
		
		private void BindInstanceToApartment<T>(T instance) where T : class
			=> Container.BindInstance(instance).WhenInjectedInto<ApartmentZone>();

#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_scenarioPlayer = FindObjectOfType<ApartmentInsideScenarioPlayer>(true);
			_interactableFinder = FindObjectOfType<PlayerInteractableFinder>(true);
			_camerasSwitcher = FindObjectOfType<ApartmentCamerasSwitcher>(true);
			_apartment = FindObjectOfType<ApartmentZone>(true);
			_inside = FindObjectOfType<ApartmentInside>(true);
			_jumpscare = FindObjectOfType<Jumpscare>(true);
			
			_apartmentOutsides.Clear();
			_apartmentOutsides.AddRange(FindObjectsOfType<ApartmentOutside>(true));
		}
#endif
	}
}