using FAS.Apartments;
using UnityEngine;
using Zenject;

namespace FAS.Players.States
{
	public class LookAtZone : PlayerState
	{
		[SerializeField] private float _cameraRotationSpeed = 10f;
		
		[Inject] private IApartmentWindow _apartmentWindow;
		
		private bool _isSwiping;
		
		public override bool IsPlayerControlledState { get; } = false;
		
		public override PlayerStates Key => PlayerStates.LookAtZone;
		
		public override void Enter()
		{
			InputEvents.OnStopLookingIntoWindowButtonClicked += ExitToTransition;
			_isSwiping = false;
		}
		
		private void ExitToTransition()
		{
			RequestTransition(PlayerStates.LookTransition);
		}
		
		public override void Perform()
		{
			if (!_apartmentWindow.IsHasJumpscare)
			{
				if (Input.IsMouseButtonDown)
					_isSwiping = true;
				else if (Input.IsMouseButtonUp)
					_isSwiping = false;

				if (_isSwiping)
					_apartmentWindow.RotateWindowCamera(
						Input.MouseDelta.x * _cameraRotationSpeed * Time.deltaTime);	
			}
		}

		public override void Exit()
		{
			UIScreensSwitcher.HideAll();
			InputEvents.OnStopLookingIntoWindowButtonClicked -= ExitToTransition;
			base.Exit();
		}
	}
}