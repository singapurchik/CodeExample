using UnityEngine.EventSystems;
using FAS.Players;
using Zenject;

namespace FAS.UI
{
	public class CameraPanelSyncButton : CustomButton
	{
		[Inject] private CameraInputPanel _inputPanel;
		
		protected override void ButtonDown(PointerEventData eventData)
		{
			base.ButtonDown(eventData);
			_inputPanel.OnPointerDown(eventData);
		}
		
		protected override void ButtonUp(PointerEventData eventData)
		{
			base.ButtonUp(eventData);
			_inputPanel.OnPointerUp(eventData);
		}
	}
}