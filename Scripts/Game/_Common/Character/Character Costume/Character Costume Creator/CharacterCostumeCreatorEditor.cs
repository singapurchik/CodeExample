#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace FAS
{
	public class CharacterCostumeCreatorEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			GUILayout.Space(6);

			if (GUILayout.Button("Create Costume"))
			{
				var creator = (CharacterCostumeCreator)target;
				// ✅ Берём метод с реального типа, а не с абстрактной базы
				var method = creator.GetType()
					.GetMethod("CreateCostume", BindingFlags.NonPublic | BindingFlags.Instance);
				if (method != null)
					method.Invoke(creator, null);
				else
					Debug.LogError($"Method CreateCostume not found via reflection on {creator.GetType().Name}");
			}

			using (new EditorGUI.DisabledScope(!CanBindAll()))
			{
				if (GUILayout.Button("Bind All Parts To Bones"))
					BindAll();

				if (GUILayout.Button("Bind All Parts At Root"))
					BindAllAtRoot();
			}
		}

		private bool CanBindAll()
		{
			var creator = (CharacterCostumeCreator)target;
			return creator != null && creator.gameObject != null;
		}

		private static Transform FindDeep(Transform root, string exactName)
		{
			if (root == null || string.IsNullOrEmpty(exactName)) return null;
			if (root.name == exactName) return root;
			var children = root.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < children.Length; i++)
				if (children[i].name == exactName)
					return children[i];
			return null;
		}

		private void BindAll()
		{
			var creator = (CharacterCostumeCreator)target;
			var go = creator.gameObject;
			var stage = PrefabStageUtility.GetCurrentPrefabStage();

			// ✅ Поддержка и Prefab Stage, и обычной сцены
			Transform rootParent = stage != null ? stage.prefabContentsRoot.transform : go.transform;

			var holder = go.GetComponentInChildren<HumanoidBonesHolder>(true);
			if (holder == null) return;

			var so = new SerializedObject(creator);
			so.Update();

			// ✅ Новые имена + fallback на старые
			var listProp = so.FindProperty("PartInsideBones") ?? so.FindProperty("_partInsideBones");
			if (listProp == null || listProp.arraySize == 0) return;

			Undo.IncrementCurrentGroup();
			int undoGroup = Undo.GetCurrentGroup();

			for (int i = 0; i < listProp.arraySize; i++)
			{
				var elem = listProp.GetArrayElementAtIndex(i);
				var nameProp = elem.FindPropertyRelative("Name");
				var partProp = elem.FindPropertyRelative("Part");
				var boneProp = elem.FindPropertyRelative("TargetBone");

				var part = partProp.objectReferenceValue as Transform;
				if (part == null) continue;

				var nestedRoot = PrefabUtility.GetNearestPrefabInstanceRoot(part.gameObject);
				if (nestedRoot == null) continue;

				var asset = PrefabUtility.GetCorrespondingObjectFromSource(nestedRoot);
				if (asset == null) continue;

				var copyRoot = (GameObject)PrefabUtility.InstantiatePrefab(asset, rootParent);
				if (copyRoot == null) continue;

				var unpackRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(copyRoot) ?? copyRoot;
				PrefabUtility.UnpackPrefabInstance(unpackRoot, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);

				var clonedPart = FindDeep(unpackRoot.transform, part.name);
				if (clonedPart == null)
				{
					Undo.DestroyObjectImmediate(unpackRoot);
					continue;
				}

				var bone = holder.GetBoneByType((FAS.HumanoidBoneType)boneProp.enumValueIndex);
				if (bone == null)
				{
					Undo.DestroyObjectImmediate(unpackRoot);
					continue;
				}

				var wPos = part.position;
				var wRot = part.rotation;
				var wScl = part.lossyScale;

				clonedPart.SetParent(bone, true);
				clonedPart.position = wPos;
				clonedPart.rotation = wRot;

				var ps = bone.lossyScale;
				clonedPart.localScale = new Vector3(
					ps.x != 0 ? wScl.x / ps.x : wScl.x,
					ps.y != 0 ? wScl.y / ps.y : wScl.y,
					ps.z != 0 ? wScl.z / ps.z : wScl.z
				);

				partProp.objectReferenceValue = clonedPart;
				if (nameProp != null) nameProp.stringValue = clonedPart.name;

				Undo.DestroyObjectImmediate(unpackRoot);
				Undo.DestroyObjectImmediate(part.gameObject);
			}

			so.ApplyModifiedProperties();
			EditorUtility.SetDirty(creator);

			// ✅ Марка сцены для обоих случаев
			if (stage != null)
			{
				EditorSceneManager.MarkSceneDirty(stage.scene);
				PrefabUtility.RecordPrefabInstancePropertyModifications(stage.prefabContentsRoot);
			}
			else
			{
				EditorSceneManager.MarkSceneDirty(go.scene);
				PrefabUtility.RecordPrefabInstancePropertyModifications(go);
			}

			Undo.CollapseUndoOperations(undoGroup);
		}

		private void BindAllAtRoot()
		{
			var creator = (CharacterCostumeCreator)target;
			var go = creator.gameObject;
			var stage = PrefabStageUtility.GetCurrentPrefabStage();
			Transform rootParent = stage != null ? stage.prefabContentsRoot.transform : go.transform;

			var holder = go.GetComponentInChildren<HumanoidBonesHolder>(true);
			if (holder == null) return;

			var hips = holder.GetBoneByType(FAS.HumanoidBoneType.Hips);
			if (hips == null)
			{
				Debug.LogWarning("BindAllAtRoot: Hips bone not found.");
				return;
			}

			// Родитель для «соседа Hips»
			var targetParent = hips.parent != null ? hips.parent : rootParent;

			var so = new SerializedObject(creator);
			so.Update();

			// ✅ Новое имя + fallback
			var listProp = so.FindProperty("PartAtRoot") ?? so.FindProperty("_partAtRoot");
			if (listProp == null || listProp.arraySize == 0) return;

			Undo.IncrementCurrentGroup();
			int undoGroup = Undo.GetCurrentGroup();

			for (int i = 0; i < listProp.arraySize; i++)
			{
				var elem = listProp.GetArrayElementAtIndex(i);
				var part = elem.objectReferenceValue as Transform;
				if (part == null) continue;

				var nestedRoot = PrefabUtility.GetNearestPrefabInstanceRoot(part.gameObject);
				if (nestedRoot == null) continue;

				var asset = PrefabUtility.GetCorrespondingObjectFromSource(nestedRoot);
				if (asset == null) continue;

				var copyRoot = (GameObject)PrefabUtility.InstantiatePrefab(asset, rootParent);
				if (copyRoot == null) continue;

				var unpackRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(copyRoot) ?? copyRoot;
				PrefabUtility.UnpackPrefabInstance(unpackRoot, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);

				var clonedPart = FindDeep(unpackRoot.transform, part.name);
				if (clonedPart == null)
				{
					Undo.DestroyObjectImmediate(unpackRoot);
					continue;
				}

				var wPos = part.position;
				var wRot = part.rotation;
				var wScl = part.lossyScale;

				clonedPart.SetParent(targetParent, true);
				clonedPart.position = wPos;
				clonedPart.rotation = wRot;

				var ps = targetParent.lossyScale;
				clonedPart.localScale = new Vector3(
					ps.x != 0 ? wScl.x / ps.x : wScl.x,
					ps.y != 0 ? wScl.y / ps.y : wScl.y,
					ps.z != 0 ? wScl.z / ps.z : wScl.z
				);

				elem.objectReferenceValue = clonedPart;

				Undo.DestroyObjectImmediate(unpackRoot);
				Undo.DestroyObjectImmediate(part.gameObject);
			}

			so.ApplyModifiedProperties();
			EditorUtility.SetDirty(creator);

			if (stage != null)
			{
				EditorSceneManager.MarkSceneDirty(stage.scene);
				PrefabUtility.RecordPrefabInstancePropertyModifications(stage.prefabContentsRoot);
			}
			else
			{
				EditorSceneManager.MarkSceneDirty(go.scene);
				PrefabUtility.RecordPrefabInstancePropertyModifications(go);
			}

			Undo.CollapseUndoOperations(undoGroup);
		}
	}
}
#endif
