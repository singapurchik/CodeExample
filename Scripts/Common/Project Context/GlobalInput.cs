using UnityEngine.EventSystems;
using Zenject;

namespace FAS
{
	public class GlobalInput
	{
		[Inject] private EventSystem _eventSystem;
		
		public bool IsActive { get; private set; }
		
		public void Enable()
		{
			_eventSystem.enabled = true;
			_eventSystem.sendNavigationEvents = true;
			IsActive = true;
		}

		public void Disable()
		{
			_eventSystem.sendNavigationEvents = false;
			_eventSystem.enabled = false;
			IsActive = false;
		}
	}
}