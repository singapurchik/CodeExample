using MagicaCloth2;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(MagicaCloth))]
public class MagicaClothCameraSetter : MonoBehaviour
{
	[Inject(Optional = true)] private Camera _mainCamera;

	private void Awake()
	{
		if (_mainCamera == null)
			_mainCamera = Camera.main;

		GetComponent<MagicaCloth>().SerializeData.cullingSettings.distanceCullingReferenceObject = _mainCamera.gameObject;
	}
}