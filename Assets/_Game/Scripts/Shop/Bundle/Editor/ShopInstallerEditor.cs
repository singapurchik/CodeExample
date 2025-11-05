#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Core;

namespace Shop.EditorTools
{
	[CustomEditor(typeof(ShopInstaller))]
	public sealed class ShopInstallerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var so = serializedObject;
			var t = (ShopInstaller)target;

			var dataInfosProp = so.FindProperty("_dataValueInfos");
			var plusBindingsProp = so.FindProperty("_statPlusBindings");

			bool changed = false;

			if (GUI.changed)
			{
				changed |= FixDataValueInfos(dataInfosProp);
				changed |= FixStatPlusBindings(plusBindingsProp);
			}

			EditorGUILayout.Space();
			using (new EditorGUILayout.HorizontalScope())
			{
				if (GUILayout.Button("Validate / Fix", GUILayout.Height(26)))
				{
					changed |= FixDataValueInfos(dataInfosProp);
					changed |= FixStatPlusBindings(plusBindingsProp);
				}
			}

			if (changed)
			{
				so.ApplyModifiedProperties();
				EditorUtility.SetDirty(t);
			}
		}

		private static bool FixDataValueInfos(SerializedProperty listProp)
		{
			if (listProp == null || !listProp.isArray) return false;

			bool changed = false;
			var seen = new HashSet<int>();

			for (int i = 0; i < listProp.arraySize; i++)
			{
				var el = listProp.GetArrayElementAtIndex(i);
				var obj = el.objectReferenceValue;
				if (!obj) continue;

				int id = obj.GetInstanceID();
				if (!seen.Add(id))
				{
					el.objectReferenceValue = null;
					changed = true;
				}
			}

			return changed;
		}

		private static bool FixStatPlusBindings(SerializedProperty listProp)
		{
			if (listProp == null || !listProp.isArray) return false;

			bool changed = false;
			var seenInfos = new HashSet<int>();
			var seenParams = new HashSet<object>(ReferenceEqualityComparer.Instance);

			for (int i = 0; i < listProp.arraySize; i++)
			{
				var el = listProp.GetArrayElementAtIndex(i);
				var infoProp = el.FindPropertyRelative("Info");
				var opProp = el.FindPropertyRelative("Operation");
				var paramProp = el.FindPropertyRelative("Param");

				var infoObj = infoProp?.objectReferenceValue as PlayerDataValueInfo;
				var opObj = opProp?.objectReferenceValue as PlayerDataOperation;

				if (infoObj == null || opObj == null)
					continue;

				int infoId = infoObj.GetInstanceID();
				if (!seenInfos.Add(infoId))
				{
					ClearBinding(el);
					changed = true;
					continue;
				}

				if (opObj is IOperationWithParameter withParam)
				{
					var cur = paramProp?.managedReferenceValue as IOperationParameter;

					if (cur == null || !withParam.IsSupports(cur))
					{
						var def = withParam.CreateDefaultParam();
						if (paramProp != null)
						{
							paramProp.managedReferenceValue = def;
							changed = true;
							seenParams.Add(def);
						}
					}
					else
					{
						if (!seenParams.Add(cur))
						{
							var clone = CloneParam(cur);
							paramProp.managedReferenceValue = clone;
							changed = true;
							seenParams.Add(clone);
						}
					}
				}
				else
				{
					if (paramProp != null && paramProp.managedReferenceValue != null)
					{
						paramProp.managedReferenceValue = null;
						changed = true;
					}
				}
			}

			return changed;
		}

		private static void ClearBinding(SerializedProperty bindingProp)
		{
			var infoProp = bindingProp.FindPropertyRelative("Info");
			var opProp = bindingProp.FindPropertyRelative("Operation");
			var paramProp = bindingProp.FindPropertyRelative("Param");

			if (infoProp != null) infoProp.objectReferenceValue = null;
			if (opProp != null) opProp.objectReferenceValue = null;
			if (paramProp != null) paramProp.managedReferenceValue = null;
		}

		private static IOperationParameter CloneParam(IOperationParameter src)
		{
			var json = JsonUtility.ToJson(src);
			return (IOperationParameter)JsonUtility.FromJson(json, src.GetType());
		}

		private sealed class ReferenceEqualityComparer : IEqualityComparer<object>
		{
			public static readonly ReferenceEqualityComparer Instance = new();
			public new bool Equals(object x, object y) => ReferenceEquals(x, y);
			public int GetHashCode(object obj) => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
		}
	}
}
#endif