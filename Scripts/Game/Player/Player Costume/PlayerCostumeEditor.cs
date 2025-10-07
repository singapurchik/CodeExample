#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEditor;
using UnityEngine;

namespace FAS.Players
{
    [CustomEditor(typeof(PlayerCostume))]
    public class PlayerCostumeEditor : Editor
    {
        private SerializedProperty _weaponPointsProp;
        private ReorderableList _list;

        private void OnEnable()
        {
            serializedObject.UpdateIfRequiredOrScript();
            _weaponPointsProp = serializedObject.FindProperty("_weaponPoints");
            Sanitize();
            BuildList();
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();
            Sanitize();
            DrawPropertiesExcluding(serializedObject, "m_Script", "_weaponPoints");
            GUILayout.Space(8);
            _list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        private void BuildList()
        {
            _list = new ReorderableList(serializedObject, _weaponPointsProp, true, true, true, true);

            _list.drawHeaderCallback = r => EditorGUI.LabelField(r, "Weapon Points");

            _list.elementHeightCallback = i =>
            {
                if (_weaponPointsProp == null || i < 0 || i >= _weaponPointsProp.arraySize)
                    return EditorGUIUtility.singleLineHeight + 4;
                var el = _weaponPointsProp.GetArrayElementAtIndex(i);
                var full = el?.managedReferenceFullTypename;
                if (string.IsNullOrEmpty(full))
                    return EditorGUIUtility.singleLineHeight + 4;
                return EditorGUIUtility.singleLineHeight + 2 + EditorGUI.GetPropertyHeight(el, true) + 4;
            };

            _list.drawElementCallback = (rect, i, _, __) =>
            {
                if (_weaponPointsProp == null || i < 0 || i >= _weaponPointsProp.arraySize) return;
                var el = _weaponPointsProp.GetArrayElementAtIndex(i);
                var full = el?.managedReferenceFullTypename;
                if (string.IsNullOrEmpty(full))
                {
                    EditorGUI.LabelField(rect, "(empty)", EditorStyles.miniLabel);
                    return;
                }

                var head = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(head, GetTypeShortName(full), EditorStyles.boldLabel);

                var body = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 2,
                    rect.width, EditorGUI.GetPropertyHeight(el, true));
                EditorGUI.PropertyField(body, el, GUIContent.none, true);
            };

            _list.onAddDropdownCallback = (r, l) =>
            {
                var menu = new GenericMenu();
                void Add<T>() where T : CostumeWeaponPoints, new()
                {
                    Undo.RecordObject(target, "Add Weapon Points");
                    serializedObject.UpdateIfRequiredOrScript();

                    _weaponPointsProp.arraySize++;
                    var el = _weaponPointsProp.GetArrayElementAtIndex(_weaponPointsProp.arraySize - 1);
                    el.managedReferenceValue = null;
                    el.managedReferenceValue = new T();

                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(target);
                    Repaint();
                }

                menu.AddItem(new GUIContent("Melee"), false, Add<CostumeMeleeWeaponPoints>);
                menu.AddItem(new GUIContent("Range"), false, Add<CostumeRangeWeaponPoints>);
                menu.ShowAsContext();
            };

            _list.onRemoveCallback = l =>
            {
                if (l.index < 0 || l.index >= _weaponPointsProp.arraySize) return;
                Undo.RecordObject(target, "Remove Weapon Points");
                serializedObject.UpdateIfRequiredOrScript();

                var el = _weaponPointsProp.GetArrayElementAtIndex(l.index);
                el.managedReferenceValue = null;
                _weaponPointsProp.DeleteArrayElementAtIndex(l.index);

                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
                Repaint();
            };
        }

        private void Sanitize()
        {
            if (_weaponPointsProp == null) return;
            bool changed = false;
            for (int i = _weaponPointsProp.arraySize - 1; i >= 0; i--)
            {
                var el = _weaponPointsProp.GetArrayElementAtIndex(i);
                var full = el?.managedReferenceFullTypename;
                if (string.IsNullOrEmpty(full))
                {
                    el.managedReferenceValue = null;
                    _weaponPointsProp.DeleteArrayElementAtIndex(i);
                    changed = true;
                }
            }
            if (changed)
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
                Repaint();
            }
        }

        private static string GetTypeShortName(string full)
        {
            int dot = full.LastIndexOf('.');
            return dot >= 0 && dot < full.Length - 1 ? full[(dot + 1)..] : full;
        }
    }
}
#endif