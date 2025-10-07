#if UNITY_EDITOR
using System.Collections.Generic;
using FAS.Apartments.Inside;
using System.Reflection;
using FAS.Apartments;
using UnityEditor;
using UnityEngine;
using VInspector;
using FAS.Items;
using System;
using FAS;

public class ApartmentInsideDataCreator : MonoBehaviour
{
	 private ApartmentInside _inside;
	 private ApartmentInsideData.RoomData _hall;
	 private ApartmentInsideData.RoomData _bedroom;
	 private readonly List<SimpleAnimatedLootContainer> _lootContainers = new();
	
	private void Create()
	{
		_inside = GetComponentInChildren<ApartmentInside>(true);
		if (_inside == null)
		{
			Debug.LogWarning("ApartmentInside not found.");
			return;
		}

		Transform hall = null;
		Transform bedroom = null;

		var allTransforms = _inside.GetComponentsInChildren<Transform>(true);
		foreach (var t in allTransforms)
		{
			if (t.name == "Hall") hall = t;
			else if (t.name == "Bedroom") bedroom = t;
		}

		if (hall == null)
		{
			Debug.LogWarning("Hall transform not found inside ApartmentInside.");
		}
		else
		{
			var (wallMat, wallTex) = GetRoomMaterialData(hall, "WallIn_", 1);
			var (floorMat, floorTex) = GetRoomMaterialData(hall, "Floor", 0);
			_hall = new ApartmentInsideData.RoomData(wallMat, wallTex, floorMat);
		}

		if (bedroom == null)
		{
			Debug.LogWarning("Bedroom transform not found inside ApartmentInside.");
		}
		else
		{
			var (wallMat, wallTex) = GetRoomMaterialData(bedroom, "WallIn_", 1);
			var (floorMat, floorTex) = GetRoomMaterialData(bedroom, "Floor", 0);
			_bedroom = new ApartmentInsideData.RoomData(wallMat, wallTex, floorMat);
		}

		_lootContainers.Clear();
		_lootContainers.AddRange(_inside.GetComponentsInChildren<SimpleAnimatedLootContainer>(true));
	}

	private (Material, Texture) GetRoomMaterialData(Transform room, string prefix, int materialIndex)
	{
		var renderers = room.GetComponentsInChildren<Renderer>(true);
		foreach (var renderer in renderers)
		{
			var name = renderer.name;
			if (name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
			{
				var materials = renderer.sharedMaterials;
				if (materials.Length > materialIndex && materials[materialIndex] != null)
				{
					var mat = materials[materialIndex];
					return (mat, mat.mainTexture);
				}
			}
		}
		return (null, null);
	}

	[Button]
	private void SaveTo(ApartmentInsideData insideData)
	{
		Create();
		
		if (insideData == null)
		{
			Debug.LogWarning("ApartmentInsideData reference is missing.");
			return;
		}

		var targetData = insideData;

		if (!EditorUtility.DisplayDialog(
			    "Confirm Save",
			    "This will overwrite the existing ApartmentInsideData. Continue?",
			    "Yes", "No"))
		{
			Debug.Log("Save cancelled.");
			return;
		}

		var lootList = new List<LootContainerData>();
		foreach (var container in _lootContainers)
		{
			var type = container.GetType();
			var hasLoot = type.GetField("_isHasLoot", BindingFlags.NonPublic | BindingFlags.Instance);
			var lootItem = type.GetField("_lootingItemData", BindingFlags.NonPublic | BindingFlags.Instance);

			if (hasLoot != null && (bool)hasLoot.GetValue(container))
			{
				var item = lootItem?.GetValue(container) as ItemData;
				lootList.Add(new LootContainerData(container.transform.name, container.UniqueId, item));
			}
		}

		Undo.RecordObject(targetData, "Populate ApartmentInsideData");

		var dataType = typeof(ApartmentInsideData);
		dataType.GetProperty("LootContainers", BindingFlags.Instance | BindingFlags.Public)?.SetValue(targetData, lootList);
		dataType.GetProperty("HallData", BindingFlags.Instance | BindingFlags.Public)?.SetValue(targetData, _hall);
		dataType.GetProperty("BedroomData", BindingFlags.Instance | BindingFlags.Public)?.SetValue(targetData, _bedroom);

		EditorUtility.SetDirty(targetData);

		Debug.Log($"ApartmentInsideData saved with {lootList.Count} loot items and room material data.");
	}

	
	[Button]
	private void LoadFrom(ApartmentInsideData insideData)
	{
		Create();
		
		if (_inside == null)
		{
			Debug.LogWarning("ApartmentInside not assigned.");
			return;
		}

		if (insideData == null)
		{
			Debug.LogWarning("ApartmentInsideData is null.");
			return;
		}

		Transform hall = null;
		Transform bedroom = null;

		var allTransforms = _inside.GetComponentsInChildren<Transform>(true);
		foreach (var t in allTransforms)
		{
			if (t.name == "Hall") hall = t;
			else if (t.name == "Bedroom") bedroom = t;
		}

		if (hall != null)
		{
			ApplyMaterialInEditor(hall, "WallIn_", 1, insideData.HallData.WallMaterial);
			ApplyMaterialInEditor(hall, "Floor", 0, insideData.HallData.FloorMaterial);
		}
		else
		{
			Debug.LogWarning("Hall transform not found.");
		}

		if (bedroom != null)
		{
			ApplyMaterialInEditor(bedroom, "WallIn_", 1, insideData.BedroomData.WallMaterial);
			ApplyMaterialInEditor(bedroom, "Floor", 0, insideData.BedroomData.FloorMaterial);
		}
		else
		{
			Debug.LogWarning("Bedroom transform not found.");
		}
	}

	private void ApplyMaterialInEditor(Transform root, string prefix, int materialIndex, Material newMaterial)
	{
		if (newMaterial == null) return;

		var renderers = root.GetComponentsInChildren<Renderer>(true);
		foreach (var renderer in renderers)
		{
			if (!renderer.name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) continue;

			var materials = renderer.sharedMaterials;
			if (materials.Length > materialIndex && materials[materialIndex] != newMaterial)
			{
				Undo.RecordObject(renderer, "Apply Room Material");
				materials[materialIndex] = newMaterial;
				renderer.sharedMaterials = materials;
				EditorUtility.SetDirty(renderer);
			}
		}
	}
}
#endif