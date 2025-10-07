using System;

namespace FAS.Apartments
{
	public interface IApartmentWindow
	{
		public bool IsHasJumpscare { get; }

		public event Action OnJumpscareShown;
		
		public void RotateWindowCamera(float delta);
	}
}