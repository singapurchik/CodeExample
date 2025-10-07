#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

public static class CleanupOldRegistryComponents
{
    [MenuItem("Tools/FAS/Cleanup Old Registry Components")]
    public static void Run()
    {
        RemoveFromAllPrefabs();
        RemoveFromAllScenes();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Cleanup done.");
    }

    static void RemoveFromAllPrefabs()
    {
        var prefabGuids = AssetDatabase.FindAssets("t:Prefab");
        foreach (var guid in prefabGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var root = PrefabUtility.LoadPrefabContents(path);
            if (root == null) continue;

            int removed = 0;
            foreach (var comp in root.GetComponentsInChildren<Component>(true))
            {
                if (comp == null) continue;
                var t = comp.GetType();
                if (t.FullName == "FAS.Players.RangeWeaponInventoryModels"
                    || t.FullName == "FAS.Players.MeleeWeaponInventoryModels"
                    || t.FullName == "FAS.Players.ItemInventoryModels")
                {
                    Object.DestroyImmediate(comp, true);
                    removed++;
                }
            }

            if (removed > 0)
            {
                PrefabUtility.SaveAsPrefabAsset(root, path);
                Debug.Log($"Removed {removed} legacy registry components from prefab: {path}");
            }
            PrefabUtility.UnloadPrefabContents(root);
        }
    }

    static void RemoveFromAllScenes()
    {
        var sceneGuids = AssetDatabase.FindAssets("t:Scene");
        foreach (var guid in sceneGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);

            int removed = 0;
            foreach (var go in scene.GetRootGameObjects())
                removed += RemoveFromHierarchy(go.transform);

            if (removed > 0)
            {
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
                Debug.Log($"Removed {removed} legacy registry components from scene: {path}");
            }
        }
    }

    static int RemoveFromHierarchy(Transform root)
    {
        int removed = 0;
        foreach (var comp in root.GetComponents<Component>())
        {
            if (comp == null) continue;
            var t = comp.GetType();
            if (t.FullName == "FAS.Players.RangeWeaponInventoryModels"
                || t.FullName == "FAS.Players.MeleeWeaponInventoryModels"
                || t.FullName == "FAS.Players.ItemInventoryModels")
            {
                Object.DestroyImmediate(comp, true);
                removed++;
            }
        }
        for (int i = 0; i < root.childCount; i++)
            removed += RemoveFromHierarchy(root.GetChild(i));
        return removed;
    }
}
#endif