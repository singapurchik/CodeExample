using Cinemachine;
using UnityEngine;
using FAS.Utils;

namespace FAS.Apartments
{
	public class ApartmentCamerasSwitcher : MonoBehaviour
	{
		[SerializeField] private CinemachineVirtualCamera _bedroomCamera;
		[SerializeField] private CinemachineVirtualCamera _windowCamera;
		[SerializeField] private CinemachineVirtualCamera _hallCamera;

		public void SwitchToBedroom()
		{
			_bedroomCamera.gameObject.TryEnable();
			_windowCamera.gameObject.TryDisable();
		}
		
		public void SwitchToWindow()
		{
			_bedroomCamera.gameObject.TryDisable();
			_windowCamera.gameObject.TryEnable();
			_hallCamera.gameObject.TryDisable();
		}

		public void SwitchToHall()
		{
			_bedroomCamera.gameObject.TryDisable();
			_windowCamera.gameObject.TryDisable();
			_hallCamera.gameObject.TryEnable();
		}
	}
}