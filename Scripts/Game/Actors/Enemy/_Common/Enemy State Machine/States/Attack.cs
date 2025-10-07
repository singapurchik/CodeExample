using UnityEngine;

namespace FAS.Actors.Emenies
{
	public class Attack : EnemyState
	{
		[SerializeField] private float _attackDistance = 2;
		[SerializeField] private float _damage = 1;
		
		public override EnemyStates Key => EnemyStates.Attack;
		
		public override void Enter()
		{
			AnimEvents.OnMeleeAttack += OnStartAttack;
			AnimEvents.OnDealDamage += TryTakeDamage;
			Animator.PlayAttackAnim();
			Mover.TryStopMove();
		}

		private void OnStartAttack()
		{
			SoundEffects.PlayMeleeAttackSound();
		}

		private void TryTakeDamage()
		{
			if (TargetDetector.IsTargetDetected && IsTargetNear(_attackDistance))
				TargetDetector.CurrentTarget.DamageReceiver.TryTakeDamage(_damage, DamageDealerType.MeleeWeapon);
		}

		public override void Perform()
		{
			if (TargetDetector.IsTargetDetected)
				RotateToTarget();

			if (Animator.AttackLayer.IsInTransition)
				RequestTransition(EnemyStates.Idle);
		}


		public override void Exit()
		{
			base.Exit();
			AnimEvents.OnDealDamage -= TryTakeDamage;
			AnimEvents.OnMeleeAttack -= OnStartAttack;
		}
	}
}