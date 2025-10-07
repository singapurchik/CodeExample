using System.Collections;
using FAS.Actors.Emenies;
using UnityEngine;
using FAS.Utils;
using Zenject;
using FAS.UI;

namespace FAS.Apartments.Inside.Scenarios
{
	public class GeishaScenario : ScenarioWithSetActiveObjects<ApartmentInsideScenarioType>
	{
		[SerializeField] private Jumpscare _jumpscare;
		[SerializeField] private Enemy _geisha;
		
		[Inject] private IUIScreensGroupSwitcher _uiScreensSwitcher;
		
		private Coroutine _currentJumpscareRoutine;
		private Quaternion _geishaDefaultRotation;
		private Vector3 _geishaDefaultPosition;
		
		public override ApartmentInsideScenarioType Type => ApartmentInsideScenarioType.Geisha;
		
		private const float JUMPSCARE_DELAY = 0.5f;

		private void Awake()
		{
			_geishaDefaultRotation = _geisha.transform.rotation;
			_geishaDefaultPosition = _geisha.transform.position;
		}
		
		protected override void Assemble()
		{
			_geisha.transform.SetPositionAndRotation(_geishaDefaultPosition, _geishaDefaultRotation);
			base.Assemble();
		}

		protected override void Disassemble()
		{
			if (_currentJumpscareRoutine != null) 
				StopCoroutine(_currentJumpscareRoutine);
			
			base.Disassemble();
		}

		public void TryShowJumpscare()
		{
			if (IsActive)
			{
				if (_currentJumpscareRoutine != null) 
					StopCoroutine(_currentJumpscareRoutine);
			
				_currentJumpscareRoutine = StartCoroutine(ShowJumpscareRoutine());	
			}
		}
		
		private IEnumerator ShowJumpscareRoutine()
		{
			_geisha.gameObject.TryDisable();
			_uiScreensSwitcher.HideAll();
			yield return new WaitForSeconds(JUMPSCARE_DELAY);
			_jumpscare.PlayScreamingToWindow();
		}
	}
}