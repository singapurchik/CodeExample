using Cinemachine;
using FAS.Sounds;
using System;

namespace FAS.Transitions
{
	[Serializable]
	public struct TransitionZoneData
	{
		public TransitionType TransitionType;
		public CameraMoveType CameraMoveType;
		public ZoneType TargetZone;
		public CinemachineBlenderSettings CameraMoveOutBlends;
		public CinemachineBlenderSettings CameraMoveInBlends;
		public SoundEvent FinishSound;
		public SoundEvent StartSound;
	}
}