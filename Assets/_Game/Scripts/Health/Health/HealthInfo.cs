using UnityEngine;
using Core;

namespace Health
{
	[CreateAssetMenu(fileName = "Health Info", menuName = "Health/Health Info")]
	public class HealthInfo : PlayerDataValueInfo
	{
		[SerializeField] private PlayerDataKey _currentHealthKey;
		
		public PlayerDataKey CurrentHealthKey => _currentHealthKey;
		
		public override string ReadCurrentValueAsString(IPlayerDataInfo data) => GetCurrentHealth(data).ToString();
		
		public int GetCurrentHealth(IPlayerDataInfo data) => data.GetInt(_currentHealthKey);
	}
}