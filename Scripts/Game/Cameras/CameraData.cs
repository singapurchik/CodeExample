using System;

namespace FAS
{
	[Serializable]
	public struct CameraData
	{
		public CameraType CameraType;
		public CameraMoveType CameraMoveType;
		public OverrideFollowCameraSide OverrideFollowCameraSide;
	}
	
	public enum CameraType
	{
		Other = 0,
		Death = 1,
		DefenderFace = 2,
	}
	
	public enum CameraMoveType
	{
		Dynamic = 0,
		Fixed = 1
	}

	public enum OverrideFollowCameraSide
	{
		None = 0,
		Back = 1,
		Front = 2,
		Left = 3,
		Right = 4
	}
}