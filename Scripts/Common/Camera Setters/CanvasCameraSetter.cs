using UnityEngine;
using Zenject;

[RequireComponent(typeof(Canvas))]
public class CanvasCameraSetter : MonoBehaviour
{
	[Inject(Optional = true)] private Camera _mainCamera;

	private void Awake()
	{
		if (_mainCamera == null)
			_mainCamera = Camera.main;

		GetComponent<Canvas>().worldCamera = _mainCamera;
		Destroy(this);
	}
}
