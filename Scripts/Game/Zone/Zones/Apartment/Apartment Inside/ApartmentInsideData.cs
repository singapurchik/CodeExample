using System.Collections.Generic;
using UnityEngine;
using System;
using FAS.Apartments.Inside.Scenarios;

namespace FAS.Apartments
{
	[CreateAssetMenu(fileName = "Apartment Inside Data", menuName = "FAS/Apartment Data",  order = 0)]
	public class ApartmentInsideData : ScriptableObject
	{
		[Serializable]
		public struct RoomData
		{
			public Material WallMaterial;
			public Texture WallAlbedo;
			public Material FloorMaterial;
			public bool IsHashPaperOnFloor;

			public RoomData(Material wallMaterial, Texture wallAlbedo, Material floorMaterial)
			{
				WallMaterial = wallMaterial;
				WallAlbedo = wallAlbedo;
				FloorMaterial = floorMaterial;
				IsHashPaperOnFloor = false;
			}
		}

		[field: SerializeField] public List<LootContainerData> LootContainers { get; private set; } = new ();
		[field: SerializeField] public RoomData BedroomData { get; private set; }
		[field: SerializeField] public RoomData HallData { get; private set; }
		[SerializeField] private ApartmentInsideScenarioType _scenario = ApartmentInsideScenarioType.Default;

		public ApartmentInsideScenarioType Scenario => _scenario;
		
		public HashSet<string> LootedContainerIDs { get; private set; } = new (10);

		public void AddLootedContainerID(string id) => LootedContainerIDs.Add(id);

		public void RemoveLootContainer(string id)
		{
			var containerToRemove = LootContainers.Find(container => container.ID == id);
			LootContainers.Remove(containerToRemove);
		}
	}
}