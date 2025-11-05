using UnityEngine;

namespace Core
{
	public abstract class PlayerDataValue : ScriptableObject
	{
		public abstract PlayerDataValueInfo Info { get; }
		
		public abstract void Initialize(PlayerData data);
	}
}