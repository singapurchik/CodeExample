using System.Collections;
using FAS.Transitions;
using Cinemachine;
using UnityEngine;
using FAS.Sounds;
using Zenject;

namespace FAS.Players.States
{
	public abstract class TransitionZoneState : PlayerState
	{
		[Inject] protected ISoundEffectsPlayer SoundEffectsPlayer { get; private set; }
		[Inject] protected IZonesHolder ZonesHolder { get; private set; }

		private Coroutine _changeCameraBlendRoutine;
		
		private bool _isHasTransition;
		
		protected ITransitionZoneCamera CurrentCamera { get; private set; }
		protected TransitionZoneData TransitionData { get; private set; }
		protected ITransitionZoneCamera LastCamera { get; private set; }
		protected ITransitionZone TransitionZone { get; private set; }
		protected IZone CurrentZone { get; private set; }
		protected IZone TargetZone { get; private set; }

		protected abstract bool IsUseSelfCameraForOut { get; }
		
		public override bool IsPlayerControlledState => false;
		
		public void UpdateData(ITransitionZone transition)
		{
			if (transition == null)
			{
				_isHasTransition = false;
			}
			else
			{
				if (CurrentZone != null && CurrentZone.IsReturnable)
					LastCamera = CurrentCamera;
				
				TransitionZone = transition;
				TransitionData = TransitionZone.Data;
				CurrentZone = ZonesHolder.CurrentZone;
				TargetZone = ZonesHolder.GetZoneByType(TransitionData.TargetZone);
				CurrentCamera = TransitionZone.Camera;
				
				if (CurrentCamera.CurrentRoutine != null)
					StopCoroutine(CurrentCamera.CurrentRoutine);
				
				_isHasTransition = true;
			}
		}

		public override void Enter()
		{
			GamePause.TryPause();
			
			if (_changeCameraBlendRoutine != null)
				StopCoroutine(_changeCameraBlendRoutine);

			if (_isHasTransition)
				OnEnterToState();
			else
				StateReturner.TryReturnLastControlledState();
		}

		protected virtual void OnEnterToState()
		{
			Renderer.Hide();
			
			if (TransitionData.CameraMoveType == Transitions.CameraMoveType.In)
				MoveInTransition();
			else
				MoveOutTransition();
		}

		protected void MoveOutTransition()
		{
			CameraBlendsChanger.TryChangeCameraBlends(TransitionData.CameraMoveOutBlends);
			
			if (IsUseSelfCameraForOut)
			{
				CurrentCamera.OnExitFinished.AddListener(OnMoveOutComplete);
				CurrentCamera.Exit();
			}
			else
			{
				LastCamera.OnExitFinished.AddListener(OnMoveOutComplete);
				LastCamera.Exit();
			}

			OnStartTransition();
		}
		
		protected virtual void MoveInTransition()
		{
			CurrentCamera.OnEnterFinished.AddListener(OnMoveInComplete);
			CameraBlendsChanger.TryChangeCameraBlends(TransitionData.CameraMoveInBlends);
			StartCoroutine(CurrentCamera.Enter());
			OnStartTransition();
		}

		protected virtual void OnStartTransition()
		{
			PursuersHolder.Clear();
		}
		
		protected virtual void OnMoveOutComplete()
		{
			if (IsUseSelfCameraForOut)
			{
				CurrentCamera.OnExitFinished.RemoveListener(OnMoveOutComplete);
				CurrentCamera.Disable();
			}
			else
			{
				LastCamera.OnExitFinished.RemoveListener(OnMoveOutComplete);
				LastCamera.Disable();
			}
		}

		protected virtual void OnMoveInComplete()
		{
			CurrentCamera.OnEnterFinished.RemoveListener(OnMoveInComplete);
			CurrentCamera.Disable();
		}
		
		public override void Exit()
		{
			if (_isHasTransition)
				OnExitFromState();
			
			base.Exit();
			GamePause.TryPlay();
		}
		
		protected virtual void OnExitFromState()
		{
			_changeCameraBlendRoutine = StartCoroutine(WaitingPhysicsUpdateAfterExit());
		}
		
		private IEnumerator WaitingPhysicsUpdateAfterExit()
		{
			yield return new WaitForFixedUpdate();
			yield return new WaitForFixedUpdate();
			yield return new WaitForFixedUpdate();
			OnFinishWaitingPhysicsUpdateAfterExit();
		}

		protected virtual void OnFinishWaitingPhysicsUpdateAfterExit()
		{
			CameraBlendsChanger.TryChangeCameraBlends(TargetZone.CameraBlends);
		}
	}
}