using Cinemachine;

namespace FAS
{
	public interface ICinemachineBrainInfo
	{
		public ICinemachineCamera ActiveCamera { get; }
		
		public bool IsBlending(out float normalizedTime, out float weight);

		public bool IsBlending();
	}
}