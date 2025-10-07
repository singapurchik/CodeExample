#if UNITY_EDITOR
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System;
using FAS;

[CustomEditor(typeof(ClothesWetness))]
public class ClothesWetnessEditor : Editor
{
    [Serializable]
    private class RendererValues
    {
	    public float WetMetallic;
	    public float WetSmoothness;
    }

    [Serializable]
    private class SaveEntry
    {
	    public string scenePath;
	    public string objectPath;
	    public List<RendererValues> values;
    }

    [Serializable]
    private class SaveWrapper { public List<SaveEntry> entries = new(); }
    
    private Transform _rootForSearch;
    
    private const string PrefsKey = "ClothesWetnessSwitcher_SaveBuffer";

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        TryRestoreIfNeeded();
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(6);

        var switcher = (ClothesWetness)target;

        _rootForSearch = (Transform)EditorGUILayout.ObjectField(
            "Root for FindRenderers",
            _rootForSearch,
            typeof(Transform),
            true);

        if (GUILayout.Button("Find Renderers (from Root)"))
        {
            var method = typeof(ClothesWetness).GetMethod("FindRenderers",
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (method != null)
            {
                var root = _rootForSearch != null ? _rootForSearch : switcher.transform;
                method.Invoke(switcher, new object[] { root });
            }
            else
            {
                Debug.LogError("Method FindRenderers not found via reflection");
            }
        }
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
            SaveAllMarkedComponents();
        else if (state == PlayModeStateChange.EnteredEditMode)
            TryRestoreIfNeeded();
    }

    private void SaveAllMarkedComponents()
    {
	    var all = FindObjectsOfType<ClothesWetness>(true);
	    var wrapper = new SaveWrapper();

	    foreach (var comp in all)
	    {
		    if (!comp || !comp.enabled) continue;

		    var saveField = typeof(ClothesWetness)
			    .GetField("SaveInPlayMode", BindingFlags.Instance | BindingFlags.NonPublic);
		    bool save = saveField != null && (bool)saveField.GetValue(comp);
		    if (!save) continue;

		    var values = new List<RendererValues>();
		    var renderersField = typeof(ClothesWetness)
			    .GetField("_renderers", BindingFlags.Instance | BindingFlags.NonPublic);

		    if (renderersField.GetValue(comp) is Array rendererArray)
		    {
			    foreach (var r in rendererArray)
			    {
				    if (r == null) continue;
				    var type = r.GetType();
				    var wetMetallic = (float)type.GetField("WetMetallic").GetValue(r);
				    var wetSmooth   = (float)type.GetField("WetSmoothness").GetValue(r);
				    values.Add(new RendererValues { WetMetallic = wetMetallic, WetSmoothness = wetSmooth });
			    }
		    }

		    var entry = new SaveEntry
		    {
			    scenePath  = comp.gameObject.scene.path,
			    objectPath = GetTransformPath(comp.transform),
			    values     = values
		    };
		    wrapper.entries.Add(entry);
	    }

	    EditorPrefs.SetString(PrefsKey, JsonUtility.ToJson(wrapper));
    }

    private void TryRestoreIfNeeded()
    {
	    var payload = EditorPrefs.GetString(PrefsKey, string.Empty);
	    if (string.IsNullOrEmpty(payload)) return;

	    var wrapper = new SaveWrapper();
	    try { JsonUtility.FromJsonOverwrite(payload, wrapper); }
	    catch {}

	    if (wrapper.entries != null)
	    {
		    foreach (var e in wrapper.entries)
		    {
			    var scene = SceneManager.GetSceneByPath(e.scenePath);
			    if (!scene.IsValid() || !scene.isLoaded) continue;

			    var comp = FindByPath(scene, e.objectPath);
			    if (comp == null) continue;

			    var renderersField = typeof(ClothesWetness)
				    .GetField("_renderers", BindingFlags.Instance | BindingFlags.NonPublic);

			    if (renderersField.GetValue(comp) is Array rendererArray)
			    {
				    for (int i = 0; i < Mathf.Min(rendererArray.Length, e.values.Count); i++)
				    {
					    var r = rendererArray.GetValue(i);
					    if (r == null) continue;

					    var type = r.GetType();
					    type.GetField("WetMetallic").SetValue(r, e.values[i].WetMetallic);
					    type.GetField("WetSmoothness").SetValue(r, e.values[i].WetSmoothness);
				    }
			    }

			    EditorUtility.SetDirty(comp);
			    PrefabUtility.RecordPrefabInstancePropertyModifications(comp);
		    }

		    AssetDatabase.SaveAssets();
	    }

	    EditorPrefs.DeleteKey(PrefsKey);
    }

    private static string GetTransformPath(Transform t)
    {
        return t.parent == null ? t.name : $"{GetTransformPath(t.parent)}/{t.name}";
    }

    private static ClothesWetness FindByPath(Scene scene, string path)
    {
        var roots = scene.GetRootGameObjects();
        if (roots == null || roots.Length == 0) return null;

        foreach (var root in roots)
        {
            var found = FindInChildrenByPath(root.transform, path);
            if (found != null) return found;
        }
        return null;
    }

    private static ClothesWetness FindInChildrenByPath(Transform root, string path)
    {
        var parts = path.Split('/');
        if (parts.Length == 0) return null;

        if (root.name != parts[0]) return null;

        Transform current = root;
        for (int i = 1; i < parts.Length; i++)
        {
            current = current.Find(parts[i]);
            if (current == null) return null;
        }

        return current.GetComponent<ClothesWetness>();
    }
}
#endif