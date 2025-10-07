using UnityEngine;
using VInspector;
using Zenject;

namespace FAS
{
	public class DamageableHealth : Health
	{
		[SerializeField] private bool _isAtoInjectView = true;
		[HideIf(nameof(_isAtoInjectView))]
		[SerializeField] private DamageableHealthView _manualAddView;
		[EndIf]
		
		[InjectOptional] private DamageableHealthView _view;

		protected override void Awake()
		{
			if (!_isAtoInjectView && _view == null)
				_view = _manualAddView;
			
			base.Awake();
		}

		public void HideView() => _view.Hide();
		
		public void ShowView()
		{
			_view.Show();
			_view.UpdateHealthText(Value, MaxHealth);
		}

		public void ResetView()
		{
			var healthNormalized = Mathf.InverseLerp(0, MaxHealth, Value);
			_view.UpdateFill(1- healthNormalized);
			
			if (healthNormalized > 0)
				_view.HideSkullImage();
			else
				_view.ShowSkullImage();
		}
		
		public void PredictHealth(float damage)
		{
			var predictedHealthNormalized = Mathf.InverseLerp(0, MaxHealth, Value - damage);
			_view.UpdateFill(1 - predictedHealthNormalized);
			
			if (predictedHealthNormalized > 0)
				_view.HideSkullImage();
			else
				_view.ShowSkullImage();
		}
	}
}