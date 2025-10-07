using UnityEngine.InputSystem.OnScreen;
using UnityEngine;

namespace FAS
{
	public class CustomOnScreenStick : OnScreenStick
	{
		protected override void OnDisable()
		{
			((RectTransform)transform).anchoredPosition = Vector2.zero;
			SendValueToControl(Vector2.zero);
			base.OnDisable();
		}
	}	
}