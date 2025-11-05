using UnityEngine;
using Core;

namespace Gold
{
	[CreateAssetMenu(fileName = "Gold", menuName = "Gold/Gold")]
	public class Gold : PlayerDataValue
	{
		[SerializeField] private GoldInfo _info;
		[SerializeField] private int _startGold = 100;
		
		public override PlayerDataValueInfo Info => _info;
		
		public override void Initialize(PlayerData data) => data.SetInt(_info.CurrentGoldKey, _startGold);
		
		public void Increase(PlayerData data, int amount)
			=> data.SetInt(_info.CurrentGoldKey, _info.GetCurrentGold(data) + amount);
		
		public void Decrease(PlayerData data, int amount)
			=> data.SetInt(_info.CurrentGoldKey, _info.GetCurrentGold(data) - amount);
		
		public int GetCurrent(IPlayerDataInfo data) =>_info.GetCurrentGold(data);

#if UNITY_EDITOR
		private void OnValidate()
		{
			_startGold = Mathf.Max(0, _startGold);
		}
#endif
	}
}