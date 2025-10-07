#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;

namespace FAS.Tools
{
	public static class AssignUniqueIdsTool
	{
		[MenuItem("Tools/FAS/Assign Unique IDs To Scene Objects")]
		public static void AssignMissingIds()
		{
			int updated = 0;

			var components = GameObject.FindObjectsOfType<UniqueIdComponent>(true);

			foreach (var component in components)
			{
				var so = new SerializedObject(component);
				var idProp = so.FindProperty("_uniqueId");

				if (string.IsNullOrEmpty(idProp.stringValue))
				{
					idProp.stringValue = Guid.NewGuid().ToString("N");
					so.ApplyModifiedProperties();
					updated++;
				}
			}

			Debug.Log($"Assigned {updated} missing Unique IDs in scene.");
		}
	}
}
#endif