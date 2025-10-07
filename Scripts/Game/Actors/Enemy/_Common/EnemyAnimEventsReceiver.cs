using UnityEngine;
using Zenject;
using System;

namespace FAS.Actors.Emenies
{
	public class EnemyAnimEventsReceiver : MonoBehaviour
	{
		[Inject] private EnemyAnimator _enemyAnimator;


		public event Action OnMeleeAttack;
		public event Action OnDealDamage;
		public event Action OnFootstep;

		private void AE_Footstep()
		{
			if (_enemyAnimator.FatalityLayer.Weight < 0.1f)
				OnFootstep?.Invoke();
		}
		
		private void AE_MeleeAttack() => OnMeleeAttack?.Invoke();
		
		private void AE_DealDamage() => OnDealDamage?.Invoke();
	}
}