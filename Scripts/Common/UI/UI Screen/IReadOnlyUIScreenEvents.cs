using UnityEngine.Events;

namespace FAS.UI
{
	public interface IReadOnlyUIScreenEvents
	{
		void AddOnStartShowListener(UnityAction listener);
		void RemoveOnStartShowListener(UnityAction listener);

		void AddOnStartHideListener(UnityAction listener);
		void RemoveOnStartHideListener(UnityAction listener);

		void AddOnShownListener(UnityAction listener);
		void RemoveOnShownListener(UnityAction listener);

		void AddOnHiddenListener(UnityAction listener);
		void RemoveOnHiddenListener(UnityAction listener);
	}
}