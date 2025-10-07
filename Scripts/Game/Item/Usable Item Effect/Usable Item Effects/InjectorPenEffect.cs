using UnityEngine;
using Zenject;

namespace FAS.Items
{
	public class InjectorPenEffect : UsableItemEffect
	{
		[SerializeField] private int _injectorPenDamageAmount = 1;
		[SerializeField] private int _injectorPenHealAmount = 1;
		[SerializeField] private int _useTimesWithoutDamage = 2;
		[Range(0, 1)] [SerializeField] private float _injectorPenDamageChance = 0.2f;

		[Inject] private IDamageReceiver _damageReceiver;
		[Inject] private IHealReceiver _healReceiver;

		private double _damageChanceCoefficient;
		
		private int _failedDamageAttempts;
		private int _useCount;
		
		public override ItemType Type => ItemType.InjectorPen;
		
		private void Awake()
		{
			_damageChanceCoefficient = PseudoRandomDistribution.GetValveCoefficientCached(_injectorPenDamageChance);
		}

		public override void Play()
		{
			if (_useCount < _useTimesWithoutDamage)
			{
				_healReceiver.TryHeal(_injectorPenHealAmount, HealDealerType.InjectorPen);
				_useCount++;
			}
			else
			{
				bool isTakeDamageThisTime =
					PseudoRandomDistribution.TryValve(_damageChanceCoefficient, ref _failedDamageAttempts);

				if (isTakeDamageThisTime)
					_failedDamageAttempts = 0;
				else
					_failedDamageAttempts++;

				if (isTakeDamageThisTime)
					_damageReceiver.TryTakeDamage(_injectorPenDamageAmount, DamageDealerType.InjectorPen);
				else
					_healReceiver.TryHeal(_injectorPenHealAmount, HealDealerType.InjectorPen);	
			}
		}
	}
}