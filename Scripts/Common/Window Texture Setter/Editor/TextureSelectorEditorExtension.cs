using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class TextureSelectorEditorExtension
{
	static TextureSelectorEditorExtension()
	{
		EditorApplication.update += RestoreAfterPlayMode;
	}

	private static bool _wasPlaying;

	private static void RestoreAfterPlayMode()
	{
		if (!EditorApplication.isPlayingOrWillChangePlaymode && _wasPlaying)
		{
			_wasPlaying = false;
			ApplyAllWindowTextures();
		}

		if (EditorApplication.isPlaying)
		{
			_wasPlaying = true;
		}
	}

	private static void ApplyAllWindowTextures()
	{
		foreach (var selector in Object.FindObjectsOfType<TextureSelector>())
		{
			selector.ApplySelectedTexture();
			SceneView.RepaintAll();
		}
	}
}