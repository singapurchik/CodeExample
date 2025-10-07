using UnityEngine.EventSystems;
using System.Globalization;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace FAS.UI
{
	public class CooldownButton : CustomButton
	{
		[SerializeField] private float _cooldownTime = 1f;
		[SerializeField] private Image _cooldownImage;
		[SerializeField] private TextMeshProUGUI _timerText;

		private float _coolDownRemainingTime;
		private float _cooldownFinishTime;
		
		private bool _isCooldown;

		private void Start()
		{
			FinishCooldown();
		}

		private void StartCooldown()
		{
			_cooldownFinishTime = Time.timeSinceLevelLoad + _cooldownTime;
			_cooldownImage.enabled = true;
			_timerText.enabled = true;
			_timerText.text = _cooldownTime.ToString(CultureInfo.InvariantCulture);
			_isCooldown = true;
		}
		
		private void ProcessCooldown()
		{
			_coolDownRemainingTime = Mathf.Max(_cooldownFinishTime - Time.timeSinceLevelLoad, 0);
			_cooldownImage.fillAmount = _coolDownRemainingTime / _cooldownTime;
			_timerText.text = Mathf.FloorToInt(_coolDownRemainingTime + 1).ToString(CultureInfo.InvariantCulture);
			
			if (IsInteractable)
				IsInteractable = false;
		}

		private void FinishCooldown()
		{
			_cooldownImage.enabled = false;
			_timerText.enabled = false;
			_isCooldown = false;
			IsInteractable = true;
			OnButtonUp?.Invoke();
		}
		
		protected override void ButtonDown(PointerEventData eventData)
		{
			StartCooldown();
			OnButtonDown?.Invoke();
		}

		private void LateUpdate()
		{
			if (_isCooldown)
			{
				if (Time.timeSinceLevelLoad < _cooldownFinishTime)
					ProcessCooldown();
				else
					FinishCooldown();
			}
		}
	}
}