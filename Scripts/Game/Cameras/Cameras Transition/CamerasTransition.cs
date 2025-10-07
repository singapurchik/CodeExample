using Cinemachine;
using UnityEngine;
using FAS.Utils;

namespace FAS
{
	public class CamerasTransition : MonoBehaviour
	{
		[SerializeField] private CinemachineVirtualCamera _cameraTransit;
		[SerializeField] private float _moveSpeed = 5f;

		private CinemachineTrackedDolly _dolly;
		
		private float _pathPosition;
		
		private int _waypointCount;

		private bool _isMoveRequested;
		private bool _isMoveForward;

		private void Awake()
		{
			_dolly = _cameraTransit.GetCinemachineComponent<CinemachineTrackedDolly>();
			_dolly.m_PathPosition = _pathPosition;
			_waypointCount = ((CinemachineSmoothPath)_dolly.m_Path).m_Waypoints.Length - 1;
		}

		public void MoveForward()
		{
			_isMoveRequested = true;
			_isMoveForward = true;
			_cameraTransit.gameObject.TryEnable();
		}

		public void MoveBackward()
		{
			_isMoveRequested = true;
			_isMoveForward = false;
			_cameraTransit.gameObject.TryEnable();
		}

		private void Update()
		{
			if (_isMoveRequested)
			{
				if (_isMoveForward)
					_pathPosition = Mathf.MoveTowards(_pathPosition, _waypointCount, _moveSpeed * Time.deltaTime);
				else
					_pathPosition = Mathf.MoveTowards(_pathPosition, 0, _moveSpeed * Time.deltaTime);
				
				_dolly.m_PathPosition = _pathPosition;
			
				if (_pathPosition == _waypointCount || _pathPosition == 0)
				{
					_cameraTransit.gameObject.TryDisable();
					_isMoveRequested = false;
				}
			}
		}
	}	
}