using UnityEngine;

namespace FAS
{
	public interface IReadOnlyInput
	{
		public Vector2 MouseVelocity { get;}
		public Vector2 MouseDelta { get;}

		public bool IsMovementJoystickActive { get;}
		public bool IsCrouchButtonEnabled { get;}
		public bool IsButtonsInputActive { get;}
		public bool IsMouseButtonDown { get;}
		public bool IsMouseButtonUp { get;}
		public bool IsMouseHeld { get;}
        
		public Vector2 GetJoystickDirection2D();
		
		public Vector3 GetJoystickDirection3D();
	}
}