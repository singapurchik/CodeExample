using UnityEngine;

public class TextureSelector : MonoBehaviour
{
	[SerializeField] private bool _isApplyingOnAwake = true;
	[SerializeField] private Renderer[] _renderers;
	[SerializeField] private TextureSelectorOption[] _options;
	[SerializeField] private int _selectedIndex;
	[SerializeField] private TexturePropertyTarget _targetProperty = TexturePropertyTarget.BaseMap;
	[SerializeField] private string _customPropertyName = "_CustomTex";

	private const string WINDOW_FRAME_PROPERTY = "_WindowFrame";
	private const string BASE_MAP_PROPERTY = "_BaseMap";
	private const string MAIN_TEX_PROPERTY = "_MainTex";

	private void Awake()
	{
		if (_isApplyingOnAwake)
			ApplySelectedTexture();
	}
	
	private string GetTargetPropertyName()
	{
		return _targetProperty switch
		{
			TexturePropertyTarget.BaseMap => BASE_MAP_PROPERTY,
			TexturePropertyTarget.MainTex => MAIN_TEX_PROPERTY,
			TexturePropertyTarget.WindowFrame => WINDOW_FRAME_PROPERTY,
			TexturePropertyTarget.Custom => _customPropertyName,
			_ => BASE_MAP_PROPERTY
		};
	}

	public void ApplySelectedTexture()
	{
		if (_renderers == null || _renderers.Length == 0 || _options == null || _options.Length == 0
		    || _selectedIndex >= _options.Length)
			return;

		SetAndApplyTexture(_options[_selectedIndex].Texture);
	}

	public void SetAndApplyTexture(Texture texture)
	{
		var block = new MaterialPropertyBlock();

		foreach (var renderer in _renderers)
		{
			renderer.GetPropertyBlock(block);
			block.SetTexture(GetTargetPropertyName(), texture);
			renderer.SetPropertyBlock(block);
		}
	}

	public Texture GetSelectedTexture() => _options[_selectedIndex].Texture;
	
	public void SetSelectedIndex(int index) => _selectedIndex = index;
}