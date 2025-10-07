using Cinemachine;

namespace FAS.Players
{
	public interface ICameraShaker
	{
		public void PlayImpulse(CinemachineImpulseSource impulseSource, float power = 1);
	}
}