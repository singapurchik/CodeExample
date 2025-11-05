using UnityEngine;

namespace Core
{
	public abstract class PlayerDataValueInfo : ScriptableObject
	{
		[SerializeField] private string _displayName = "Name";
		
		public string DisplayName => _displayName;
		
		public abstract string ReadCurrentValueAsString(IPlayerDataInfo data);
	}
}