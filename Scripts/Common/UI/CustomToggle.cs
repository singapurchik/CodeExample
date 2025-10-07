using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace FAS.UI
{
	public class CustomToggle : CustomButton
	{
		[SerializeField] private Image _toggleImage;
		[SerializeField] private Sprite _disabledSprite;
		[SerializeField] private Sprite _enabledSprite;
		[SerializeField] private bool _isEnabledOnAwake;
		
		public bool IsEnabled { get; private set; }

		[ShowIf(nameof(IsEventsShowInEditor))]
		public UnityEvent OnStateSwitched;
		public UnityEvent OnDisabled;
		public UnityEvent OnEnabled;
		[EndIf]

		protected override void Awake()
		{
			base.Awake();
			
			if (_isEnabledOnAwake)
				IsEnabled = true;
			
			if (IsEnabled)
				SilentEnable();
			else
				SilentDisable();
		}

		protected override void OnClickComplete()
		{
			base.OnClickComplete();
			SwitchState();
		}

		private void SwitchState()
		{
			if (IsEnabled)
				Disable();
			else
				Enable();
			
			OnStateSwitched?.Invoke();
		}

		public virtual void SilentEnable()
		{
			_toggleImage.sprite = _enabledSprite;
			IsEnabled = true;
		}
		
		public virtual void SilentDisable()
		{
			_toggleImage.sprite = _disabledSprite;
			IsEnabled = false;
		}
		
		public void Enable()
		{
			SilentEnable();
			OnEnabled?.Invoke();
		}

		public void Disable()
		{
			SilentDisable();
			OnDisabled?.Invoke();
		}
		
		public void ForceDisable() => SilentDisable();

	}
}