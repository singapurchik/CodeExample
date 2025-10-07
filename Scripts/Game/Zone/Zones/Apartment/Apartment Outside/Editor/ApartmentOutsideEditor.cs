using FAS.Apartments;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FAS.Apartments.Outside.ApartmentOutside))]
public class ApartmentOutsideEditor : Editor
{
	private Editor _insideEditor;
	private bool _showInsideData = true;

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		DrawDefaultInspector();

		if (!Application.isPlaying)
		{
			var insideProp = serializedObject.FindProperty("_insideData");
			var inside = insideProp.objectReferenceValue as ApartmentInsideData;

			if (inside != null)
			{
				EditorGUILayout.Space(10);

				_showInsideData = EditorGUILayout.Foldout(_showInsideData, "Apartment Inside Data", 
					true, EditorStyles.foldoutHeader);

				if (_showInsideData)
				{
					EditorGUI.indentLevel++;

					if (_insideEditor == null || _insideEditor.target != inside)
						_insideEditor = CreateEditor(inside);

					_insideEditor.OnInspectorGUI();

					EditorGUI.indentLevel--;

					if (GUI.changed)
					{
						EditorUtility.SetDirty(inside);
						AssetDatabase.SaveAssets();
					}
				}
			}
		}

		serializedObject.ApplyModifiedProperties();
	}
}