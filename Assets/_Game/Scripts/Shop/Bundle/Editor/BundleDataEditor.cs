#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Shop.EditorTools
{
	[CustomEditor(typeof(BundleData))]
	public sealed class BundleDataEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var bd = (BundleData)target;

			EditorGUILayout.Space();
			using (new EditorGUILayout.HorizontalScope())
			{
				if (GUILayout.Button("Validate / Fix", GUILayout.Height(26)))
				{
					if (BundleDataValidator.Validate(bd))
					{
						EditorUtility.SetDirty(bd);
						AssetDatabase.SaveAssets();
						Debug.Log($"[BundleData] '{bd.name}' validated & fixed.");
					}
					else
					{
						Debug.Log($"[BundleData] '{bd.name}' is OK.");
					}
				}
			}

			if (GUI.changed)
			{
				if (BundleDataValidator.Validate(bd))
					EditorUtility.SetDirty(bd);
			}
		}
	}
}
#endif