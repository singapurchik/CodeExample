using UnityEngine;

namespace FAS.Players.States
{
	public class RangeWeaponAiming : RangeWeaponState
	{
		[SerializeField] private float _minAutoRotationSpeed = 6f;
		[SerializeField] private float _maxAutoRotationSpeed = 24f;

		[SerializeField] private float _minAngle = 5f;
		[SerializeField] private float _maxAngle = 90f;

		private bool _isSwiping;
		
		public override PlayerStates Key => PlayerStates.RangeWeaponAiming;
		public override bool IsPlayerControlledState => true;
		
		public override void Enter()
		{
			InputEvents.OnFinishAimingButtonClicked += RequestRangeWeaponStow;
			InputEvents.OnShootButtonClicked += RequestRangeWeaponShoot;
			TargetFinder.OnTargetChanged += OnTargetChanged;
			TargetFinder.SetEnabled(true);
			Animator.PlayStartAiming();
			ScreenGroup.Show();
			_isSwiping = false;

			if (TargetFinder.IsHasTarget)
				OnTargetChanged(TargetFinder.CurrentTarget);
		}

		private void OnTargetChanged(IRangeWeaponTarget target) => target.PredictHealth(Weapon.Info.Damage);

		private void RequestRangeWeaponShoot() => RequestTransition(PlayerStates.RangeWeaponShooting);
		
		private void RequestRangeWeaponStow() => RequestTransition(PlayerStates.RangeWeaponStow);

		public override void Perform()
		{
			TargetFinder.RequestFindClosestTarget();
			
			if (Input.IsMouseButtonDown)
				_isSwiping = true;
			else if (Input.IsMouseButtonUp)
				_isSwiping = false;

			if (_isSwiping && Mathf.Abs(Input.MouseDelta.x) > 0.01f)
				TargetFinder.TrySwitchTargetByMouseDelta(Input.MouseDelta.x);

			if (TargetFinder.IsHasTarget)
			{
				var toTarget = TargetFinder.CurrentTargetPosition - transform.position;
				toTarget.y = 0f;

				var sqrMagnitude = toTarget.sqrMagnitude;
				if (sqrMagnitude > 0.000001f)
				{
					var directionToTarget = toTarget.normalized;

					var forward = transform.forward;
					forward.y = 0f;
					forward.Normalize();

					var angle = Vector3.Angle(forward, directionToTarget);
					var autoSpeed = GetAutoRotationSpeedByAngle(angle);

					Rotator.TryRotateToTargetHorizontal(TargetFinder.CurrentTargetPosition, autoSpeed);
				}
			}
		}
		
		private float GetAutoRotationSpeedByAngle(float angle)
		{
			var factor = Mathf.InverseLerp(_minAngle, _maxAngle, angle);
			var speed = Mathf.Lerp(_minAutoRotationSpeed, _maxAutoRotationSpeed, factor);
			return speed;
		}

		public override void Exit()
		{
			base.Exit();
			TargetFinder.SetEnabled(false);
			UIScreensSwitcher.HideAll();
			Animator.PlayFinishAiming();
			TargetFinder.OnTargetChanged -= OnTargetChanged;
			InputEvents.OnShootButtonClicked -= RequestRangeWeaponShoot;
			InputEvents.OnFinishAimingButtonClicked -= RequestRangeWeaponStow;
		}
	}
}