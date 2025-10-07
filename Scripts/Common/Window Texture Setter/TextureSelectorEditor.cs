#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextureSelector))]
public class TextureSelectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var selector = (TextureSelector)target;

        var isApplyingOnAwakeProp = serializedObject.FindProperty("_isApplyingOnAwake");
        var renderersProp = serializedObject.FindProperty("_renderers");
        var optionsProp = serializedObject.FindProperty("_options");
        var indexProp = serializedObject.FindProperty("_selectedIndex");
        var enumProp = serializedObject.FindProperty("_targetProperty");
        var customProp = serializedObject.FindProperty("_customPropertyName");

        EditorGUILayout.PropertyField(isApplyingOnAwakeProp, new GUIContent("Is Applying On Awake"));
        EditorGUILayout.PropertyField(renderersProp, new GUIContent("Renderers"), true);
        EditorGUILayout.PropertyField(optionsProp, new GUIContent("Texture Options"), true);

        int count = optionsProp.arraySize;

        if (count > 0)
        {
            string[] labels = new string[count];
            for (int i = 0; i < count; i++)
            {
                var optionProp = optionsProp.GetArrayElementAtIndex(i);
                var so = optionProp.objectReferenceValue as TextureSelectorOption;
                labels[i] = so != null && !string.IsNullOrEmpty(so.DisplayName) ? so.DisplayName : $"Option {i}";
            }

            int selectedIndex = Mathf.Clamp(indexProp.intValue, 0, count - 1);
            indexProp.intValue = selectedIndex;

            int newIndex = EditorGUILayout.Popup("Selected Texture", selectedIndex, labels);
            if (newIndex != selectedIndex)
            {
                indexProp.intValue = newIndex;
                serializedObject.ApplyModifiedProperties();
                selector.ApplySelectedTexture();
                EditorUtility.SetDirty(selector);
            }

            var selectedOption = (TextureSelectorOption)optionsProp.GetArrayElementAtIndex(indexProp.intValue).objectReferenceValue;
            if (selectedOption?.Texture != null)
            {
                GUILayout.Label("Preview", EditorStyles.boldLabel);
                float size = Mathf.Min(EditorGUIUtility.currentViewWidth - 40, 128);
                Rect rect = GUILayoutUtility.GetRect(size, size, GUILayout.ExpandWidth(false));
                EditorGUI.DrawPreviewTexture(rect, selectedOption.Texture);
            }
        }
        else
        {
            indexProp.intValue = 0;
            EditorGUILayout.HelpBox("No options assigned.", MessageType.Info);
        }

        EditorGUILayout.PropertyField(enumProp, new GUIContent("Target Property"));
        if ((TexturePropertyTarget)enumProp.enumValueIndex == TexturePropertyTarget.Custom)
        {
            EditorGUILayout.PropertyField(customProp, new GUIContent("Custom Name"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif