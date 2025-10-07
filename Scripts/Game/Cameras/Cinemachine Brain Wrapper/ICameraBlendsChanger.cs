using Cinemachine;

namespace FAS
{
	public interface ICameraBlendsChanger
	{
		public void TryChangeCameraBlends(CinemachineBlenderSettings blends);

		public void TryReturnLastBlends();
	}
}