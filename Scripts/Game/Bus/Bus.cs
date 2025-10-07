using UnityEngine.Events;
using UnityEngine;

namespace FAS
{
	public class Bus : MonoBehaviour, IInteractableVisitable
	{
		[SerializeField] private bool _isDoorLocked = true;
		
		public bool IsDoorLocked => _isDoorLocked;

		public UnityEvent OnDoorUnlocked;
		
		public void UnlockDoor()
		{
			OnDoorUnlocked?.Invoke();
		}

		public void Accept(IInteractableVisitor visitor) => visitor.Apply(this);
	}	
}