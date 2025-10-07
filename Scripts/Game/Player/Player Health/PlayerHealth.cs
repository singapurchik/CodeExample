using Zenject;

namespace FAS.Players
{
	public class PlayerHealth : Health
	{
		[Inject] private PlayerReactionEffects _reaction;
		
		public override void TryTakeDamage(float damage, DamageDealerType damageDealerType = DamageDealerType.Unknown)
		{
			switch (damageDealerType)
			{
				case DamageDealerType.InjectorPen:
					TakeDamageFromInjectorPen(damage);
					break;
				case DamageDealerType.Flies:
					TakeDamageFromFlies(damage);
					break;
				case DamageDealerType.MeleeWeapon:
				case DamageDealerType.Unknown:
				default:
					base.TryTakeDamage(damage, damageDealerType);
					break;
			}
		}

		public override void TryHeal(float heal, HealDealerType healDealerType = HealDealerType.Unknown)
		{
			switch (healDealerType)
			{
				case HealDealerType.InjectorPen:
					TakeHealFromInjectorPen(heal);
					break;
				default:
				case HealDealerType.Unknown:
					base.TryHeal(heal, healDealerType);
					break;
			}
		}

		private void TakeDamageFromInjectorPen(float damage)
		{
			ModifyHealth(-damage);
			_reaction.PlayDamageFromInjection();
		}

		private void TakeHealFromInjectorPen(float heal)
		{
			ModifyHealth(heal);
			_reaction.PlayHealFromInjection();
		}

		private void TakeDamageFromFlies(float damage)
		{
			ModifyHealth(-damage);
			_reaction.PlayDamageFromFlies();
		}
	}
}