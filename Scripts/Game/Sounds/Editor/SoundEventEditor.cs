#if UNITY_EDITOR
using FAS.Sounds.AudioMixerParametrs;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEditor;
using UnityEngine;
using FAS.Sounds;
using System;

[CustomEditor(typeof(SoundEvent))]
public class SoundEventEditor : Editor
{
    private SerializedProperty _mixerConfigsProp;
    private SerializedProperty _volumeDBProp;
    private SerializedProperty _volumeProp;
    private SerializedProperty _clipsProp;
    private ReorderableList _mixerList;

    private void OnEnable()
    {
        _clipsProp = serializedObject.FindProperty("_clips");
        _volumeProp = serializedObject.FindProperty("_volume");
        _volumeDBProp = serializedObject.FindProperty("_volumeDB");
        _mixerConfigsProp = serializedObject.FindProperty("_mixerConfigs");

        _mixerList = new ReorderableList(serializedObject, _mixerConfigsProp, 
	        true, true, true, true)
        {
            drawHeaderCallback = rect =>
                EditorGUI.LabelField(rect, "Audio Mixer Parameters"),

            elementHeightCallback = index =>
            {
                var element = _mixerConfigsProp.GetArrayElementAtIndex(index);
                float labelHeight = EditorGUIUtility.singleLineHeight + 2;
                float fieldHeight = EditorGUI.GetPropertyHeight(element, true);
                return labelHeight + fieldHeight + 4;
            },

            drawElementCallback = (rect, index, _, _) =>
            {
                var element = _mixerConfigsProp.GetArrayElementAtIndex(index);
                var typeName = GetDisplayTypeName(element.managedReferenceFullTypename);

                var labelRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(labelRect, typeName, EditorStyles.boldLabel);

                var fieldRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 2, rect.width,
                    EditorGUI.GetPropertyHeight(element, true));
                EditorGUI.PropertyField(fieldRect, element, GUIContent.none, true);
            },

            onAddDropdownCallback = (rect, list) =>
            {
                var menu = new GenericMenu();
                var types = new Dictionary<string, Type>
                {
                    { "Lowpass", typeof(LowpassParameterConfig) },
                    // Add more types here
                };

                foreach (var kvp in types)
                {
                    menu.AddItem(new GUIContent(kvp.Key), false, () =>
                    {
                        var instance = Activator.CreateInstance(kvp.Value);
                        _mixerConfigsProp.arraySize++;
                        _mixerConfigsProp.GetArrayElementAtIndex(_mixerConfigsProp.arraySize - 1).managedReferenceValue = instance;
                        serializedObject.ApplyModifiedProperties();
                    });
                }

                menu.ShowAsContext();
            }
        };
    }

    public override void OnInspectorGUI()
    {
	    serializedObject.Update();

	    EditorGUILayout.PropertyField(_clipsProp, new GUIContent("Audio Clips"), true);
	    EditorGUILayout.PropertyField(_volumeProp);
	    EditorGUILayout.PropertyField(_volumeDBProp); // ✅

	    GUILayout.Space(10);
	    _mixerList.DoLayoutList();

	    serializedObject.ApplyModifiedProperties();
    }

    private string GetDisplayTypeName(string fullTypeName)
    {
        if (string.IsNullOrEmpty(fullTypeName))
            return "(null)";

        var cleanType = fullTypeName.Split('^')[0];
        var parts = cleanType.Split('.');
        var typeName = parts.Length > 0 ? parts[^1] : cleanType;

        const string suffixToRemove = "ParameterConfig";
        if (typeName.EndsWith(suffixToRemove))
            typeName = typeName.Substring(0, typeName.Length - suffixToRemove.Length);

        return typeName;
    }
}
#endif