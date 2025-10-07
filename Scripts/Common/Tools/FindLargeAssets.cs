#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class FindLargeAssets : EditorWindow
{
	private int minSizeMB = 90;

	[MenuItem("Tools/FAS/Find Large Assets")]
	public static void ShowWindow()
	{
		GetWindow<FindLargeAssets>("Find Large Assets");
	}

	private void OnGUI()
	{
		GUILayout.Label("Search for large assets in /Assets", EditorStyles.boldLabel);
		minSizeMB = EditorGUILayout.IntSlider("Minimum size (MB)", minSizeMB, 1, 1024);

		if (GUILayout.Button($"Search (>{minSizeMB} MB)"))
		{
			FindAssetsLargerThan(minSizeMB);
		}
	}

	private void FindAssetsLargerThan(int minMB)
	{
		long thresholdBytes = minMB * 1024L * 1024L;
		string[] assetPaths = AssetDatabase.GetAllAssetPaths();

		int count = 0;
		foreach (string path in assetPaths)
		{
			if (!path.StartsWith("Assets")) continue;

			string fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);
			if (!File.Exists(fullPath)) continue;

			FileInfo fileInfo = new FileInfo(fullPath);
			if (fileInfo.Length >= thresholdBytes)
			{
				count++;
				Debug.Log($"{path} — {fileInfo.Length / (1024 * 1024)} MB", AssetDatabase.LoadMainAssetAtPath(path));
			}
		}

		Debug.Log($"Search complete. Found {count} asset(s) larger than {minMB} MB.");
	}
}
#endif