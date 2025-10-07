using System.Collections.Generic;
using FAS.Apartments.Outside;
using FAS.Apartments.Inside;
using UnityEngine;
using FAS.Sounds;
using Zenject;

namespace FAS.Apartments
{
	[RequireComponent(typeof(ApartmentInside))]
	public class ApartmentZone : Zone<ApartmentInside>, IReadOnlyApartment
	{
		[SerializeField] private Transform _enterPoint;
		[SerializeField] private float _interactableFindingRadius = 1f;

		[Inject] private IInteractableFinderRadius _interactableFinderRadius;
		[Inject] private List<ApartmentOutside> _outside;
		[Inject] private IStreetAmbience _streetAmbience;
		[Inject] private IApartmentScreensGroup _screens;
		[Inject] private IMusicChanger _musicChanger;
		
		private ApartmentOutside _currentOutside;

		public override Transform EnterPoint => _enterPoint;
		
		public override ZoneType Type => ZoneType.Apartment;

		public int CurrentApartmentNumber => _currentOutside.ApartmentNumber;
		public int ApartmentsCount => _outside.Count;

		public override bool IsReturnable { get; } = true;
		
		private void OnEnable()
		{
			foreach (var outside in _outside)
			{
				outside.OnEnterApartment += ChangeApartment;
				outside.OnEnterWindow += OnLookAtWindow;
				outside.Initialize(Inside);
			}
		}

		private void OnDisable()
		{
			foreach (var outside in _outside)
			{
				outside.OnEnterApartment -= ChangeApartment;
				outside.OnEnterWindow -= OnLookAtWindow;
			}
		}

		private void ChangeApartment(ApartmentOutside outside)
		{
			_currentOutside = outside;
			Inside.Setup(_currentOutside.InsideData, _currentOutside.WindowTexture);
		}

		private void OnLookAtWindow(ApartmentOutside outside)
		{
			_streetAmbience.PlayThroughWindow();
			ChangeApartment(outside);
			_screens.OpenWindowScreen();
			Inside.ShowFromWindow();
			_musicChanger.StopMusic();
		}
		
		public override void Enter()
		{
			_streetAmbience.PlayIndoor();
			_musicChanger.StopMusic();
			Inside.ShowFromHall();
			OnEnter?.Invoke(Type);
			IsActive = true;
		}
		
		public override void Exit()
		{
			_streetAmbience.PlayOutdoor();
			base.Exit();
		}
		
		private void Update()
		{
			if (IsActive)
				_interactableFinderRadius.RequestChangeRadius(_interactableFindingRadius);
		}
	}
}