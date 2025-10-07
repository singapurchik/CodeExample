using System.Collections.Generic;
using MagicaCloth2;
using UnityEngine;
using VInspector;

namespace FAS
{
	public class ClothPhysicsEnabler : MonoBehaviour
	{
		[SerializeField] private List<MagicaCloth> _cloths = new ();
		
		// private void OnEnable()
		// {
		// 	Settings.OnGraphicsQualityChanged += OnGraphicsQualityChanged;
		// }
		//
		// private void OnDisable()
		// {
		// 	Settings.OnGraphicsQualityChanged -= OnGraphicsQualityChanged;
		// }
		//
		// private void Start()
		// {
		// 	OnGraphicsQualityChanged(Settings.CurrentGraphicsQuality);
		// }
		//
		// private void OnGraphicsQualityChanged(GraphicsQualityType quality)
		// {
		// 	if (Settings.CurrentGraphicsQuality == GraphicsQualityType.Low)
		// 		DisableAll();
		// 	else
		// 		EnableAll();
		// }
		//
		// private void EnableAll()
		// {
		// 	foreach (var cloth in _cloths)
		// 		cloth.enabled = true;
		// }
		//
		// private void DisableAll()
		// {
		// 	foreach (var cloth in _cloths)
		// 		cloth.enabled = false;
		// }

#if UNITY_EDITOR
		[Button]
		private void FindCloths()
		{
			_cloths.Clear();
			_cloths.AddRange(GetComponentsInChildren<MagicaCloth>(true));
		}
#endif
	}
}