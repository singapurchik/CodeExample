using System;

namespace FAS
{
	public interface IReadOnlyHealth
	{
		public float Value { get; }
		
		public event Action OnHealthModified;
		public event Action OnTakeDamage;
		public event Action OnDead;
		public event Action OnHeal;
		public event Action OnFull;
	}
}