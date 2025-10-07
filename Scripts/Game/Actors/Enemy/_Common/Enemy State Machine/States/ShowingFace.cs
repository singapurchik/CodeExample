using Cinemachine;
using UnityEngine;
using Zenject;

namespace FAS.Actors.Emenies
{
	public class ShowingFace : EnemyState
	{
		[SerializeField] private float _loseSoundDelay;
		
		[Inject] private CinemachineVirtualCamera _camera;

		protected bool IsWaitingForCameraBlend;
		
		public override EnemyStates Key => EnemyStates.ShowingFace;

		public override void Enter()
		{
			IsWaitingForCameraBlend = true;
			_camera.gameObject.SetActive(true);
			SoundEffects.PlayFaceCameraFly();
		}

		protected virtual void OnCameraBlendCompleted()
		{
			SoundEffects.PlayLoseSound(_loseSoundDelay);
			IsWaitingForCameraBlend = false;
		}
		
		public override void Perform()
		{
			TargetDetector.RequestDisable();

			if (IsWaitingForCameraBlend &&
			    ActiveCameraData.Type == CameraType.DefenderFace
					&& !CinemachineBrainInfo.IsBlending()) 
				OnCameraBlendCompleted();
		}
	}
}