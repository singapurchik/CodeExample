using UnityEngine;
using FAS.Items;
using System;

namespace FAS
{
	[Serializable]
	public struct LootContainerData
	{
		[HideInInspector] public string Name;
		[HideInInspector] public string ID;
		public ItemData LootItem;

		public LootContainerData(string name, string id, ItemData item)
		{
			Name = name;
			ID = id;
			LootItem = item;
		}
	}
}