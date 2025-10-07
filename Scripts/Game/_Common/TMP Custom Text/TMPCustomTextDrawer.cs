#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FAS
{
	[CustomPropertyDrawer(typeof(TMPCustomText))]
	public class TMPCustomTextDrawer : PropertyDrawer
	{
		private static readonly GUIContent BoldLabel = new GUIContent("B", "Bold");
		private static readonly GUIContent ItalicLabel = new GUIContent("I", "Italic");
		private static readonly GUIContent UnderLabel = new GUIContent("U", "Underline");
		private static readonly GUIContent StrikeLabel = new GUIContent("S", "Strikethrough");

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		}

		public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			SerializedProperty text = property.FindPropertyRelative("Text");
			SerializedProperty color = property.FindPropertyRelative("Color");
			SerializedProperty strike = property.FindPropertyRelative("Strikethrough");
			SerializedProperty underline = property.FindPropertyRelative("Underline");
			SerializedProperty italic = property.FindPropertyRelative("Italic");
			SerializedProperty bold = property.FindPropertyRelative("Bold");

			EditorGUI.BeginProperty(rect, label, property);

			float h = EditorGUIUtility.singleLineHeight;
			float pad = 2f;

			Rect labelRect = new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, h);
			property.isExpanded = EditorGUI.Foldout(labelRect, property.isExpanded, label, true);

			float x = rect.x + EditorGUIUtility.labelWidth;
			float totalWidth = rect.width - EditorGUIUtility.labelWidth;

			float colorWidth = 70f;
			float buttonW = 22f;
			float buttonsWidth = buttonW * 4 + pad * 3;

			float textWidth = Mathf.Max(30f, totalWidth - colorWidth - buttonsWidth - pad * 3);

			Rect textRect = new Rect(x, rect.y, textWidth, h);
			x += textWidth + pad;

			Rect colorRect = new Rect(x, rect.y, colorWidth, h);
			x += colorWidth + pad;

			Rect bRect = new Rect(x, rect.y, buttonW, h);
			x += buttonW + pad;
			Rect iRect = new Rect(x, rect.y, buttonW, h);
			x += buttonW + pad;
			Rect uRect = new Rect(x, rect.y, buttonW, h);
			x += buttonW + pad;
			Rect sRect = new Rect(x, rect.y, buttonW, h);

			if (color.colorValue == default)
				color.colorValue = new Color32(249, 244, 193, 255);

			EditorGUI.PropertyField(textRect, text, GUIContent.none);
			color.colorValue = EditorGUI.ColorField(colorRect, GUIContent.none, color.colorValue, true, true, false);

			bold.boolValue = GUI.Toggle(bRect, bold.boolValue, BoldLabel, EditorStyles.miniButtonLeft);
			italic.boolValue = GUI.Toggle(iRect, italic.boolValue, ItalicLabel, EditorStyles.miniButtonMid);
			underline.boolValue = GUI.Toggle(uRect, underline.boolValue, UnderLabel, EditorStyles.miniButtonMid);
			strike.boolValue = GUI.Toggle(sRect, strike.boolValue, StrikeLabel, EditorStyles.miniButtonRight);

			EditorGUI.EndProperty();
		}
	}
}
#endif