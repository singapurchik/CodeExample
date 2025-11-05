#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Core;

namespace Shop.EditorTools
{
    [CustomPropertyDrawer(typeof(StatPlusBinding))]
    public sealed class StatPlusBindingDrawer : PropertyDrawer
    {
        private const string InfoPropName  = "Info";
        private const string OpPropName    = "Operation";
        private const string ParamPropName = "Param";

        private const float BottomSpacing = 6f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var infoProp  = property.FindPropertyRelative(InfoPropName);
            var opProp    = property.FindPropertyRelative(OpPropName);
            var paramProp = property.FindPropertyRelative(ParamPropName);

            float line = EditorGUIUtility.singleLineHeight;
            float pad  = EditorGUIUtility.standardVerticalSpacing;

            var contentRect = new Rect(position.x, position.y, position.width, position.height - BottomSpacing);

            var r1 = new Rect(contentRect.x, contentRect.y, contentRect.width, line);
            EditorGUI.PropertyField(r1, infoProp);

            var r2 = new Rect(contentRect.x, r1.yMax + pad, contentRect.width, line);
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(r2, opProp);
            bool opChanged = EditorGUI.EndChangeCheck();

            var opObj = opProp.objectReferenceValue as PlayerDataOperation;

            if (opChanged)
            {
                if (opObj is IOperationWithParameter withParam)
                {
                    var cur = paramProp?.managedReferenceValue as IOperationParameter;
                    if (cur == null || !withParam.IsSupports(cur))
                    {
                        paramProp.managedReferenceValue = withParam.CreateDefaultParam();
                        property.serializedObject.ApplyModifiedProperties();
                    }
                }
                else
                {
                    if (paramProp != null && paramProp.managedReferenceValue != null)
                    {
                        paramProp.managedReferenceValue = null;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                }
            }

            if (opObj is IOperationWithParameter && paramProp != null && paramProp.managedReferenceValue != null)
            {
                var ph = EditorGUI.GetPropertyHeight(paramProp, includeChildren: true);
                var r3 = new Rect(contentRect.x, r2.yMax + pad, contentRect.width, ph);
                EditorGUI.PropertyField(r3, paramProp, includeChildren: true);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float h   = EditorGUIUtility.singleLineHeight;
            float pad = EditorGUIUtility.standardVerticalSpacing;

            h += pad + EditorGUIUtility.singleLineHeight;

            var opProp    = property.FindPropertyRelative(OpPropName);
            var paramProp = property.FindPropertyRelative(ParamPropName);

            var opObj = opProp?.objectReferenceValue as PlayerDataOperation;
            if (opObj is IOperationWithParameter && paramProp != null && paramProp.managedReferenceValue != null)
	            h += pad + EditorGUI.GetPropertyHeight(paramProp, includeChildren: true); // Param

            h += BottomSpacing;
            return h;
        }
    }
}
#endif
