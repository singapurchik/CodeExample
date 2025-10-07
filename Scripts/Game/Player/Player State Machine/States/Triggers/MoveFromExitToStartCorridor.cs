using UnityEngine;

namespace FAS.Players.States.Triggers
{
	public class MoveFromExitToStartCorridor : PlayerState
	{
		[SerializeField] private float _moveSpeed = 20f;
		[SerializeField] private float _rotationSpeed = 720f;
		
		private Transform _finishPoint;
		private Transform _startPoint;

		public override PlayerStates Key => PlayerStates.SwitchToMoveFromExitToStartCorridor;

		public override bool IsPlayerControlledState { get; } = false;

		public void UpdateData(Transform startPoint, Transform finishPoint)
		{
			_finishPoint = finishPoint;
			_startPoint = startPoint;
		}
		
		public override void Enter()
		{
			InputControl.DisableMovementInput();
			CameraShaker.PlayStartFlyingBack();
			Animator.PlayFromSlipToFlyingBackAnim();
			UIScreensSwitcher.HideAll();
		}

		public override void Perform()
		{
			var animLayer = Animator.FullBodyReactionLayer;

			if (animLayer.CurrentAnimHash == animLayer.FlyingBackAnimHash)
			{
				Rotator.SmoothRotateHorizontal(_finishPoint.eulerAngles.y, _rotationSpeed);
				Mover.RequestTransformMove(_startPoint.position, _moveSpeed);
				
				if (Vector3.SqrMagnitude(transform.position - _startPoint.position) < 0.01f)
				{
					CameraShaker.PlayFinishFlyingBack();
					RequestTransition(PlayerStates.KnockedDown);
				}
			}
			
			Jump.UpdateGravity();
		}
	}
}