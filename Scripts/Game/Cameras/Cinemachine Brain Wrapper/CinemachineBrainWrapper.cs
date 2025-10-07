using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace FAS
{
	[RequireComponent(typeof(CinemachineBrain))]
	public class CinemachineBrainWrapper : MonoBehaviour, ICinemachineBrainInfo, ICameraBlendsChanger
	{
		private CinemachineBrain _brain;

		private readonly Stack<CinemachineBlenderSettings> _lastBlends = new (5);
		
		public ICinemachineCamera ActiveCamera => _brain.ActiveVirtualCamera;

		private void Awake()
		{
			_brain = GetComponent<CinemachineBrain>();
		}

		public bool IsBlending() => _brain.IsBlending;
		
		public bool IsBlending(out float normalizedTime, out float weight)
		{
			if (_brain.IsBlending)
			{
				var activeBlend = _brain.ActiveBlend;
				normalizedTime = Mathf.Clamp01(activeBlend.TimeInBlend / activeBlend.Duration);
				weight = activeBlend.BlendWeight;
				return true;
			}

			normalizedTime = -1f;
			weight = -1f;
			return false;
		}
		
		public void TryChangeCameraBlends(CinemachineBlenderSettings blends)
		{
			if (_brain.m_CustomBlends != blends)
			{
				_lastBlends.Push(_brain.m_CustomBlends);
				_brain.m_CustomBlends = blends;
			}
		}
		
		public void TryReturnLastBlends()
		{
			if (_lastBlends.TryPop(out var lastBlend))
				TryChangeCameraBlends(lastBlend);
		}
	}
}