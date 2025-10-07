using UnityEngine;

namespace FAS
{
	public class AxisRotator : MonoBehaviour
	{
		[SerializeField] private Vector3 _speed = new(0f, 0f, 0f);

		private void Update()
		{
			transform.Rotate(_speed * Time.deltaTime, Space.Self);
		}
	}
}