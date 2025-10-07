#if UNITY_EDITOR
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor;
using System.Linq;

public static class MissingScriptCleaner
{
	[MenuItem("Tools/FAS/Remove Missing Scripts (Scene & Prefab Mode)")]
	public static void RemoveMissingScripts()
	{
		var totalRemoved = SceneManager.GetActiveScene().GetRootGameObjects().Sum(CleanRecursive);

		var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
		
		if (prefabStage != null)
		{
			var prefabRoot = prefabStage.prefabContentsRoot;
			totalRemoved += CleanRecursive(prefabRoot);
		}

		Debug.Log($"[MissingScriptCleaner] Removed {totalRemoved} missing script(s).");
	}

	private static int CleanRecursive(GameObject go)
	{
		return GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go)
		       + go.transform.Cast<Transform>().Sum(child => CleanRecursive(child.gameObject));
	}
}
#endif