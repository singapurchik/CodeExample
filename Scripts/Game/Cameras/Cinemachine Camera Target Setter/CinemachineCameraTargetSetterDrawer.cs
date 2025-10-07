#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FAS
{
	[CustomPropertyDrawer(typeof(CinemachineCameraTargetSetter.CameraTargets), true)]
	public class CameraTargetsDrawer : PropertyDrawer
	{
		private const float Spacing = 2f;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
			property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

			if (property.isExpanded)
			{
				EditorGUI.indentLevel++;

				var y = position.y + EditorGUIUtility.singleLineHeight + Spacing;

				var isHasAimProp = property.FindPropertyRelative("_isHasAimTarget");
				var aimTargetProp = property.FindPropertyRelative("_aimTarget");
				var aimBoneProp = property.FindPropertyRelative("_aimTargetBone");

				// новые имена
				var isHasFollowProp = property.FindPropertyRelative("_isHasFollowTarget");
				var followTargetProp = property.FindPropertyRelative("_followTarget");
				var followBoneProp = property.FindPropertyRelative("_followTargetBone");

				DrawLine(ref y, position, isHasAimProp, new GUIContent("Has Aim Target"));
				if (isHasAimProp.boolValue)
				{
					DrawLine(ref y, position, aimTargetProp, new GUIContent("Aim Target"));
					if ((CinemachineCameraTargetSetter.CameraTargetType)aimTargetProp.enumValueIndex
					    == CinemachineCameraTargetSetter.CameraTargetType.PlayerBone)
					{
						DrawLine(ref y, position, aimBoneProp, new GUIContent("Aim Bone"));
					}
				}

				DrawSeparator(ref y, position);

				DrawLine(ref y, position, isHasFollowProp, new GUIContent("Has Follow Target"));
				if (isHasFollowProp.boolValue)
				{
					DrawLine(ref y, position, followTargetProp, new GUIContent("Follow Target"));
					if ((CinemachineCameraTargetSetter.CameraTargetType)followTargetProp.enumValueIndex
					    == CinemachineCameraTargetSetter.CameraTargetType.PlayerBone)
					{
						DrawLine(ref y, position, followBoneProp, new GUIContent("Follow Bone"));
					}
				}

				EditorGUI.indentLevel--;
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float h = EditorGUIUtility.singleLineHeight;
			if (!property.isExpanded) return h;

			var isHasAim = property.FindPropertyRelative("_isHasAimTarget").boolValue;
			var aimTarget = property.FindPropertyRelative("_aimTarget").enumValueIndex;
			var showAimBone = isHasAim && ((CinemachineCameraTargetSetter.CameraTargetType)aimTarget
			                               == CinemachineCameraTargetSetter.CameraTargetType.PlayerBone);

			var isHasFollow = property.FindPropertyRelative("_isHasFollowTarget").boolValue;
			var followTarget = property.FindPropertyRelative("_followTarget").enumValueIndex;
			var showFollowBone = isHasFollow && ((CinemachineCameraTargetSetter.CameraTargetType)followTarget
			                                     == CinemachineCameraTargetSetter.CameraTargetType.PlayerBone);

			int lines = 0;
			lines += 1; // has aim
			if (isHasAim) lines += 1; // aim target
			if (showAimBone) lines += 1; // aim bone

			lines += 1; // separator
			lines += 1; // has follow
			if (isHasFollow) lines += 1; // follow target
			if (showFollowBone) lines += 1; // follow bone

			h += lines * (EditorGUIUtility.singleLineHeight + Spacing);
			return h;
		}

		private static void DrawLine(ref float y, Rect total, SerializedProperty prop, GUIContent label)
		{
			var r = new Rect(total.x, y, total.width, EditorGUIUtility.singleLineHeight);
			EditorGUI.PropertyField(r, prop, label);
			y += EditorGUIUtility.singleLineHeight + Spacing;
		}

		private static void DrawSeparator(ref float y, Rect total)
		{
			y += Spacing;
			var line = new Rect(total.x + 2, y, total.width - 4, 1);
			EditorGUI.DrawRect(line, new Color(0, 0, 0, 0.2f));
			y += Spacing;
		}
	}
}
#endif