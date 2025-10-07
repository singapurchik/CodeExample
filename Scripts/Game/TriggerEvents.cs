using UnityEngine.Events;
using FAS.Players;
using UnityEngine;

namespace FAS
{
	public class TriggerEvents : MonoBehaviour
	{
		public UnityEvent OnEnter;
		public UnityEvent OnExit;


		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Player player))
				OnEnter?.Invoke();
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out Player player))
				OnExit?.Invoke();
		}
	}
}