#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public sealed class TaggedShadowBaker : EditorWindow
{
    private string _tag = "BakeShadow";
    private bool _disableAfterBake = true;

    private readonly List<Entry> _entries = new();

    private struct Entry
    {
        public GameObject Obj;
        public bool WasActive;
        public Renderer Renderer;
        public bool RendererEnabled;
        public ShadowCastingMode ShadowMode;
        public StaticEditorFlags StaticFlags;
    }

    [MenuItem("Tools/FAS/Tagged Shadow Baker")]
    private static void Open() => GetWindow<TaggedShadowBaker>("Tagged Shadow Baker");

    private void OnGUI()
    {
        _tag = EditorGUILayout.TagField("Tag", _tag);
        _disableAfterBake = EditorGUILayout.Toggle("Disable After Bake", _disableAfterBake);

        EditorGUI.BeginDisabledGroup(Lightmapping.isRunning);
        if (GUILayout.Button("Bake With Tagged Casters"))
            StartBake();
        EditorGUI.EndDisabledGroup();
    }

    private void StartBake()
    {
        _entries.Clear();

        var tagged = FindWithTagIncludingInactive(_tag);
        foreach (var obj in tagged)
        {
            var renderer = obj.GetComponent<Renderer>();
            if (!renderer) continue;

            var entry = new Entry
            {
                Obj = obj,
                WasActive = obj.activeSelf,
                Renderer = renderer,
                RendererEnabled = renderer.enabled,
                ShadowMode = renderer.shadowCastingMode,
                StaticFlags = GameObjectUtility.GetStaticEditorFlags(obj)
            };
            _entries.Add(entry);

            obj.SetActive(true);
            renderer.enabled = true;
            renderer.shadowCastingMode = ShadowCastingMode.On;

            var flags = entry.StaticFlags | StaticEditorFlags.ContributeGI | StaticEditorFlags.ContributeGI;
#if UNITY_6000_0_OR_NEWER
            // В Unity 6 есть отдельный флаг Static Shadow Caster
            flags |= (StaticEditorFlags) (1 << 21); // StaticShadowCaster (в API пока не всем виден enum)
#endif
            GameObjectUtility.SetStaticEditorFlags(obj, flags);
        }

        if (_entries.Count == 0)
        {
            Debug.LogWarning("TaggedShadowBaker: No objects with the specified tag found (including inactive).");
            return;
        }

        // Гарантируем валидные настройки освещения
        var ls = Lightmapping.lightingSettings; // бросит исключение, если LightingSettings не назначен
        ls.autoGenerate = false;
        ls.bakedGI = true;
        Lightmapping.lightingSettings = ls;

        EditorSceneManager.MarkAllScenesDirty();

        Lightmapping.bakeCompleted += OnBakeCompleted;

        if (!Lightmapping.BakeAsync())
        {
            Debug.LogWarning("TaggedShadowBaker: BakeAsync() returned false. " +
                             "Проверь: есть ли Baked/Mixed light, LightingSettings присвоен сцене, autoGenerate выключен.");
            Cleanup();
            Lightmapping.bakeCompleted -= OnBakeCompleted;
        }
    }

    private static List<GameObject> FindWithTagIncludingInactive(string tag)
    {
        var result = new List<GameObject>(128);
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (!scene.isLoaded) continue;

            var roots = scene.GetRootGameObjects();
            foreach (var root in roots)
            {
                var transforms = root.GetComponentsInChildren<Transform>(true);
                foreach (var t in transforms)
                    if (t.CompareTag(tag))
                        result.Add(t.gameObject);
            }
        }
        return result;
    }

    private void OnBakeCompleted()
    {
        Lightmapping.bakeCompleted -= OnBakeCompleted;
        Cleanup();
        Debug.Log("TaggedShadowBaker: Bake finished and original states restored.");
    }

    private void Cleanup()
    {
        foreach (var e in _entries)
        {
            if (e.Renderer)
            {
                e.Renderer.shadowCastingMode = e.ShadowMode;
                e.Renderer.enabled = e.RendererEnabled;
            }

            GameObjectUtility.SetStaticEditorFlags(e.Obj, e.StaticFlags);
            e.Obj.SetActive(_disableAfterBake ? false : e.WasActive);
        }
        _entries.Clear();
    }
}
#endif
