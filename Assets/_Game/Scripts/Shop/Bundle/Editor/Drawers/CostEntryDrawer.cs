#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Core;

namespace Shop.EditorTools
{
	[CustomPropertyDrawer(typeof(CostEntry))]
	public sealed class CostEntryDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var opProp = property.FindPropertyRelative(EntryDrawerUtil.OperationProp);
			var paramProp = property.FindPropertyRelative(EntryDrawerUtil.ParameterProp);

			float line = EditorGUIUtility.singleLineHeight;
			float pad = EditorGUIUtility.standardVerticalSpacing;
			var r1 = new Rect(position.x, position.y, position.width, line);

			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(r1, opProp);
			bool opChanged = EditorGUI.EndChangeCheck();

			if (opChanged)
			{
				if (EntryDrawerUtil.EnsureParamCompatibility(property))
					property.serializedObject.ApplyModifiedProperties();
			}

			var opObj = opProp.objectReferenceValue as PlayerDataOperation;
			if (opObj is IOperationWithParameter && paramProp != null && paramProp.managedReferenceValue != null)
			{
				var r2 = new Rect(position.x, r1.yMax + pad, position.width,
					EditorGUI.GetPropertyHeight(paramProp, true));
				EditorGUI.PropertyField(r2, paramProp, includeChildren: true);
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float h = EditorGUIUtility.singleLineHeight;
			float pad = EditorGUIUtility.standardVerticalSpacing;

			var opProp = property.FindPropertyRelative(EntryDrawerUtil.OperationProp);
			var paramProp = property.FindPropertyRelative(EntryDrawerUtil.ParameterProp);

			var opObj = opProp?.objectReferenceValue as PlayerDataOperation;
			if (opObj is IOperationWithParameter && paramProp != null && paramProp.managedReferenceValue != null)
				h += pad + EditorGUI.GetPropertyHeight(paramProp, true);

			return h;
		}
	}
}
#endif