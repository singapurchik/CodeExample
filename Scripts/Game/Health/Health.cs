using UnityEngine;
using System;

namespace FAS
{
	public class Health : MonoBehaviour, IReadOnlyHealth, IDamageReceiver, IHealReceiver
	{
		[SerializeField] protected int MaxHealth = 2;
		
		public float Value { get; private set; }
		
		public event Action OnHealthModified;
		public event Action OnTakeDamage;
		public event Action OnDead;
		public event Action OnHeal;
		public event Action OnFull;

		protected virtual void Awake()
		{
			Value = MaxHealth;
		}

		protected void ModifyHealth(float value)
		{
			Value = Mathf.Clamp(Value + value, 0, MaxHealth);
			OnHealthModified?.Invoke();
		}

		public virtual void TryTakeDamage(float damage, DamageDealerType damageDealerType = DamageDealerType.Unknown)
		{
			if (Value > 0)
			{
				Value -= damage;
				OnTakeDamage?.Invoke();

				if (Value <= 0)
				{
					Value = 0;
					OnDead?.Invoke();
				}
			}
		}
		
		public virtual void TryHeal(float heal, HealDealerType healDealerType = HealDealerType.Unknown)
		{
			if (Value < MaxHealth)
			{
				Value += heal;
				OnHeal?.Invoke();

				if (Value >= MaxHealth)
				{
					Value = MaxHealth;
					OnFull?.Invoke();
				}
			}
		}
	}
}