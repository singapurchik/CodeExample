using UnityEngine;
using FAS.Utils;

namespace FAS
{
	public class CharacterFakeShadow : MonoBehaviour, IFakeShadow
	{
		[SerializeField] private MeshRenderer _meshRenderer;

		private GameObject _rendererObject;
		
		private bool _isEnableRequested;
		
		private void Awake() => _rendererObject = _meshRenderer.gameObject;

		public void RequestEnable() => _isEnableRequested = true;

		protected virtual void Update()
		{
			if (_isEnableRequested)
			{
				_rendererObject.TryEnable();
				_isEnableRequested = false;
			}
			else
			{
				if (Settings.CurrentGraphicsQuality == GraphicsQualityType.Low)
					_rendererObject.TryEnable();
				else
					_rendererObject.TryDisable();	
			}
		}
	}
}