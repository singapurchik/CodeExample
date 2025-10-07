using Cinemachine;
using UnityEngine;

namespace FAS
{
	[RequireComponent(typeof(CinemachineVirtualCamera))]
	public class CinemachineDollyResetter : MonoBehaviour
	{
		[SerializeField] private float _resetPosition = 0f;
		
		private CinemachineTrackedDolly _dolly;

		private void Awake()
		{
			var vcam = GetComponent<CinemachineVirtualCamera>();
			_dolly = vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
		}

		private void OnEnable()
		{
			_dolly.m_PathPosition = _resetPosition;
		}

		private void OnDisable()
		{
			_dolly.m_PathPosition = _resetPosition;
		}
	}	
}
