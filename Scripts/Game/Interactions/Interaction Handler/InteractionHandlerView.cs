using UnityEngine;

namespace FAS
{
	public class InteractionHandlerView : MonoBehaviour
	{
		[SerializeField] private LookAtCamera _lookAtCamera;
		[SerializeField] private MeshRenderer _meshRenderer;
		[SerializeField] private Sprite _icon;

		private static readonly int _baseMapProperty = Shader.PropertyToID("_BaseMap");

		public Sprite Icon => _icon;

		private void Awake()
		{
			var block = new MaterialPropertyBlock();
			_meshRenderer.GetPropertyBlock(block);
			block.SetTexture(_baseMapProperty, _icon.texture);
			_meshRenderer.SetPropertyBlock(block);
		}

		public void Hide()
		{
			_lookAtCamera.enabled = false;
			_meshRenderer.enabled = false;
		}

		public void Show()
		{
			_lookAtCamera.enabled = true;
			_meshRenderer.enabled = true;
		}
	}
}