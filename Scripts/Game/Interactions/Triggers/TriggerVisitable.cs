using UnityEngine;

namespace FAS.Triggers
{
	public abstract class TriggerVisitable : MonoBehaviour, ITriggerVisitable
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out ITriggerVisitor visitor))
				Accept(visitor);
		}

		public abstract void Accept(ITriggerVisitor visitor);
	}
}