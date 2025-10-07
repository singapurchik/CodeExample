using Cinemachine;
using UnityEngine;

namespace FAS.Players
{
	public class PlayerCameraShaker : MonoBehaviour, ICameraShaker
	{
		[SerializeField] private CinemachineImpulseSource _knifeOutOfStomach;
		[SerializeField] private CinemachineImpulseSource _finishFlyingBack;
		[SerializeField] private CinemachineImpulseSource _startFlyingBack;
		[SerializeField] private CinemachineImpulseSource _leftLegSlash;
		[SerializeField] private CinemachineImpulseSource _stabStomach;
		[SerializeField] private CinemachineImpulseSource _neckSlash;
		
		public void PlayKnifeOutOfStomach(float force = 1) => _knifeOutOfStomach.GenerateImpulse(force);
		
		public void PlayFinishFlyingBack(float force = 1) => _finishFlyingBack.GenerateImpulse(force);
		
		public void PlayStartFlyingBack(float force = 1) => _startFlyingBack.GenerateImpulse(force);
		
		public void PlayLeftLegSlash(float force = 1) => _leftLegSlash.GenerateImpulse(force);
		
		public void PlayStabStomach(float force = 1) => _stabStomach.GenerateImpulse(force);
		
		public void PlayNeckSlash(float force = 1) => _neckSlash.GenerateImpulse(force);

		public void PlayImpulse(CinemachineImpulseSource impulseSource, float power = 1)
			=> impulseSource.GenerateImpulse(power);
	}
}