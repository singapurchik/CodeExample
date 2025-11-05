using UnityEngine;
using Core;

namespace Gold
{
	[CreateAssetMenu(fileName = "Gold Info", menuName = "Gold/Gold Info")]
	public class GoldInfo : PlayerDataValueInfo
	{
		[SerializeField] private PlayerDataKey _currentGoldKey;

		public PlayerDataKey CurrentGoldKey => _currentGoldKey;
		
		public override string ReadCurrentValueAsString(IPlayerDataInfo data) => GetCurrentGold(data).ToString();

		public int GetCurrentGold(IPlayerDataInfo data) => data.GetInt(_currentGoldKey);
	}
}