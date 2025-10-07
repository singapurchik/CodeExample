#if UNITY_EDITOR
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FAS.CostumePartInsideHumanoidBone))]
public class CostumePartInsideHumanoidBoneDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var nameProp = property.FindPropertyRelative("Name");
		var partProp = property.FindPropertyRelative("Part");
		var targetBoneProp = property.FindPropertyRelative("TargetBone");

		var headerRect = new Rect(position.x, position.y, position.width - 130, EditorGUIUtility.singleLineHeight);

		property.isExpanded = EditorGUI.Foldout(headerRect, property.isExpanded, label, true);

		if (!property.isExpanded) return;

		float y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

		if (nameProp != null)
		{
			var rect = new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight);
			using (new EditorGUI.DisabledScope(true))
				EditorGUI.TextField(rect, "Name", nameProp.stringValue);
			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		}

		if (partProp != null)
		{
			var h = EditorGUI.GetPropertyHeight(partProp, true);
			var rect = new Rect(position.x, y, position.width, h);
			EditorGUI.PropertyField(rect, partProp, true);
			y += h + EditorGUIUtility.standardVerticalSpacing;
		}

		if (targetBoneProp != null)
		{
			var h = EditorGUI.GetPropertyHeight(targetBoneProp, true);
			var rect = new Rect(position.x, y, position.width, h);
			EditorGUI.PropertyField(rect, targetBoneProp, true);
		}

		if (partProp != null && partProp.objectReferenceValue is Transform tr2 &&
		    nameProp != null && nameProp.stringValue != tr2.name)
		{
			Undo.RecordObject(property.serializedObject.targetObject, "Sync Name with Part");
			nameProp.stringValue = tr2.name;
			property.serializedObject.ApplyModifiedProperties();
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		float h = EditorGUIUtility.singleLineHeight;
		if (!property.isExpanded) return h;

		float total = h + EditorGUIUtility.standardVerticalSpacing;
		total += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		var partProp = property.FindPropertyRelative("Part");
		var targetBoneProp = property.FindPropertyRelative("TargetBone");
		if (partProp != null)
			total += EditorGUI.GetPropertyHeight(partProp, true) + EditorGUIUtility.standardVerticalSpacing;
		if (targetBoneProp != null)
			total += EditorGUI.GetPropertyHeight(targetBoneProp, true) + EditorGUIUtility.standardVerticalSpacing;
		return total;
	}

	private static string GetBaseName(string src)
	{
		if (string.IsNullOrEmpty(src)) return src;
		string s = Regex.Replace(src, @"\s*\(\d+\)\s*$", "");
		s = Regex.Replace(s, @"\s+\d+\s*$", "");
		int underscore = s.IndexOf('_');
		if (underscore >= 0) s = s.Substring(0, underscore);
		return s.Trim();
	}
}
#endif