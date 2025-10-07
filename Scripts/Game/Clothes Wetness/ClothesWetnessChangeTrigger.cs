using UnityEngine;
using VInspector;

namespace FAS
{
	public class ClothesWetnessChangeTrigger : ReliableTrigger<IClothesWetness>
	{
		[SerializeField] private bool _isEnableClothesWetness;

		[ShowIf(nameof(_isEnableClothesWetness))]
		[SerializeField] private bool _isForceEnableWetnessOnEnter;

		[DisableIf(nameof(_isForceEnableWetnessOnEnter))]
		[SerializeField] private float _enableWetnessSpeed = 0.25f;
		[EndIf]

		[HideIf(nameof(_isEnableClothesWetness))]
		[SerializeField] private float _disableWetnessSpeed = 2f;
		[EndIf]

		protected override void OnEnter(IClothesWetness target, Collider other)
		{
			if (_isEnableClothesWetness && _isForceEnableWetnessOnEnter)
				target.RequestForceEnableWetness();
		}

		private void Update()
		{
			if (TargetCount == 0)
				return;

			if (_isEnableClothesWetness)
			{
				if (_isForceEnableWetnessOnEnter)
				{
					foreach (var wetness in Targets)
						wetness.RequestForceEnableWetness();
				}
				else
				{
					foreach (var wetness in Targets)
						wetness.RequestEnableWetnessSmooth(_enableWetnessSpeed);
				}
			}
			else
			{
				foreach (var wetness in Targets)
					wetness.RequestChangeDisableWetnessSpeed(_disableWetnessSpeed);
			}
		}
	}
}