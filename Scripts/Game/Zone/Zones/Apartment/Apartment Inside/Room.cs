using System.Collections.Generic;
using FAS.Apartments;
using UnityEditor;
using UnityEngine;
using VInspector;
using System;

public class Room : MonoBehaviour
{
    [SerializeField] private List<Renderer> _wallsRenderers = new();
    [SerializeField] private Renderer _floorRenderer;
    [SerializeField] private List<GameObject> _paperDecals = new();

    private static readonly int _baseMapId = Shader.PropertyToID("_BaseMap");

    private MaterialPropertyBlock _propertyBlock;

    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();
    }

    public void Setup(ApartmentInsideData.RoomData data)
    {
        foreach (var paperDecal in _paperDecals)
            paperDecal.SetActive(data.IsHashPaperOnFloor);

        SetWallTexture(data.WallAlbedo);
        SetFloorTexture(data.FloorMaterial);
    }

    private void SetWallTexture(Texture albedo)
    {
        foreach (var wallRenderer in _wallsRenderers)
        {
            _propertyBlock.Clear();
            wallRenderer.GetPropertyBlock(_propertyBlock, 1);
            _propertyBlock.SetTexture(_baseMapId, albedo);
            wallRenderer.SetPropertyBlock(_propertyBlock, 1);
        }
    }

    private void SetFloorTexture(Material material)
    {
        var sourceTexture = material.mainTexture;

        _propertyBlock.Clear();
        _floorRenderer.GetPropertyBlock(_propertyBlock, 0);
        _propertyBlock.SetTexture(_baseMapId, sourceTexture);
        _floorRenderer.SetPropertyBlock(_propertyBlock, 0);
    }

#if UNITY_EDITOR
    [Button]
    private void FindDependencies()
    {
        _wallsRenderers.Clear();
        _floorRenderer = null;
        _paperDecals.Clear();

        var childRenderers = GetComponentsInChildren<Renderer>(true);
        foreach (var candidateRenderer in childRenderers)
        {
            var objectName = candidateRenderer.gameObject.name;

            if (objectName.StartsWith("WallIn_", StringComparison.OrdinalIgnoreCase))
                _wallsRenderers.Add(candidateRenderer);
            else if (_floorRenderer == null && objectName.StartsWith("Floor", StringComparison.OrdinalIgnoreCase))
                _floorRenderer = candidateRenderer;
        }

        var childTransforms = GetComponentsInChildren<Transform>(true);
        foreach (var childTransform in childTransforms)
        {
            if (childTransform.name.StartsWith("Decal_papers", StringComparison.OrdinalIgnoreCase))
                _paperDecals.Add(childTransform.gameObject);
        }

        EditorUtility.SetDirty(this);
        Debug.Log($"Room: Found {_wallsRenderers.Count} walls," +
                  $" {_paperDecals.Count} decals, floor renderer: {(_floorRenderer?.name ?? "None")}");
    }
#endif
}
