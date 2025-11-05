using UnityEngine;
using System;

namespace Core
{
	[Serializable]
	public sealed class PlayerDataKey
	{
		[HideInInspector] [SerializeField] private string _id = Guid.NewGuid().ToString("N");
		
		public string Id => _id;
	}
}