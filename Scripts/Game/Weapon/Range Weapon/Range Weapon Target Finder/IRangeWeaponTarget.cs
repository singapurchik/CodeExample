using UnityEngine;

namespace FAS
{
	public interface IRangeWeaponTarget
	{
		public Vector3 AimingPosition { get; }
		public Vector3 Position { get; }

		public void PredictHealth(float damage);
		public void Deselect();
		public void Select();
	}
}