using UnityEngine;

namespace FAS.UI
{
	public class FadingFrameScreen : UIScreen, IUIFadingFrame
	{
		[SerializeField] private FadingFrame _jumpscareFrame;
		[SerializeField] private FadingFrame _easyBloodFrame;
		[SerializeField] private FadingFrame _hardBloodFrame;
		[SerializeField] private FadingFrame _healFrame;

		public void PlayJumpscare() => _jumpscareFrame.Play();
		
		public void PlayHardBlood() => _hardBloodFrame.Play();

		public void PlayEasyBlood() => _easyBloodFrame.Play();
		
		public void PlayHeal() => _healFrame.Play();
	}
}