using UnityEngine;

namespace Core
{
	public abstract class PlayerDataOperation : ScriptableObject, IPlayerDataOperationInfo
	{
		public abstract PlayerDataValueInfo Info { get; }
		
		public abstract bool IsCanApply(IPlayerDataInfo data);
		
		public abstract void Apply(PlayerData data);
	}
}