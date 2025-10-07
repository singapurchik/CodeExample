using UnityEngine;

namespace FAS.Actors.Companion
{
	public class KnockedBack : CompanionState
	{
		[SerializeField] private float _rotateToTargetSpeed = 10f;
		
		public override GirlStates Key =>  GirlStates.KnockedBack;

		private bool _isKnockedBackAnimPlaying;
		
		public override void Enter()
		{
			_isKnockedBackAnimPlaying = false;
			Mover.TryStopMove();
			Animator.PlayKnockedBackAnim();
		}

		public override void Perform()
		{
			Rotator.RequestRotateHorizontal(
				Quaternion.LookRotation(GetHorizontalDirectionToOwner()).eulerAngles.y, _rotateToTargetSpeed);
			
			Mover.RequestEnableRootMotion();

			if (_isKnockedBackAnimPlaying)
			{
				if (Animator.BaseLayer.IsInTransition)
					RequestTransition(GirlStates.Idle);
			}
			else if (Animator.BaseLayer.CurrentAnimHash == Animator.BaseLayer.KnockedBackAnimHash)
			{
				_isKnockedBackAnimPlaying = true;
			}
		}
	}
}