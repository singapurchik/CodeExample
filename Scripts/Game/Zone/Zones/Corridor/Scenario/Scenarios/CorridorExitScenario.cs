using System.Collections.Generic;
using FAS.Triggers.Corridor;
using FAS.Apartments;
using UnityEngine;
using Zenject;

namespace FAS.Corridor
{
	public class CorridorExitScenario : Scenario<CorridorScenarioTypes>
	{
		[SerializeField] private MoveFromExitToStartCorridor _moveFromExitToStartCorridorTrigger;
		[SerializeField] private GameObject _fakeExitDoor;
		[SerializeField] private GameObject _exitDoor;

		[Inject] private IReadOnlyApartment _apartment;
		[Inject] private List<CorridorDoor> _doors;
		
		public override CorridorScenarioTypes Type => CorridorScenarioTypes.ExitFromCorridor;
		
		protected override void Assemble()
		{
			foreach (var door in _doors)
				door.OnEnter += OnDoorEnter;
			
			ShowFakeExit();
			base.Assemble();
		}

		protected override void Disassemble()
		{
			foreach (var door in _doors)
				door.OnEnter -= OnDoorEnter;
			
			base.Disassemble();
		}

		private void ShowFakeExit()
		{
			_moveFromExitToStartCorridorTrigger.gameObject.SetActive(true);
			_fakeExitDoor.SetActive(true);
			_exitDoor.SetActive(false);
			ChangeDoorsNumbers();
		}
		
		private void ChangeDoorsNumbers()
		{
			int maxApartmentNumber = _apartment.ApartmentsCount + 1;
			
			var allNumbers = new List<int>(maxApartmentNumber);
			
			for (int i = 1; i <= maxApartmentNumber; i++)
				allNumbers.Add(i);

			for (int i = allNumbers.Count - 1; i > 0; i--)
			{
				int j = Random.Range(0, i + 1);
				(allNumbers[i], allNumbers[j]) = (allNumbers[j], allNumbers[i]);
			}

			var chosen = allNumbers.GetRange(0, _doors.Count);

			if (!chosen.Contains(_apartment.CurrentApartmentNumber))
			{
				int replaceIndex = Random.Range(0, chosen.Count);
				chosen[replaceIndex] = _apartment.CurrentApartmentNumber;
			}

			for (int i = 0; i < _doors.Count; i++)
				_doors[i].SetNumber(chosen[i]);
		}

		private void ShowExit()
		{
			_moveFromExitToStartCorridorTrigger.gameObject.SetActive(false);
			_fakeExitDoor.SetActive(false);
			_exitDoor.SetActive(true);
		}

		private void OnDoorEnter(int doorNumber)
		{
			if (doorNumber == _apartment.CurrentApartmentNumber)
				ShowExit();
			else
				ShowFakeExit();
		}
	}
}