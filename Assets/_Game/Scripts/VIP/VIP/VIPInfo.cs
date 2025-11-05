using UnityEngine;
using System;
using Core;

namespace VIP
{
	[CreateAssetMenu(fileName = "VIP Info", menuName = "VIP/VIP Info")]
	public class VIPInfo : PlayerDataValueInfo
	{
		[SerializeField] private PlayerDataKey _vipRemainingTimeKey;
		public PlayerDataKey VipRemainingTimeKey => _vipRemainingTimeKey;

		public long GetUntilTicks(IPlayerDataInfo data)
			=> VipTime.TryReadUntil(data, _vipRemainingTimeKey, out var t) ? t : 0L;

		public TimeSpan GetRemainingTime(IPlayerDataInfo data)
		{
			var until = GetUntilTicks(data);
			var left  = until - VipTime.NowTicks();
			return left > 0 ? TimeSpan.FromTicks(left) : TimeSpan.Zero;
		}

		public int GetRemainingSeconds(IPlayerDataInfo data)
		{
			var until = GetUntilTicks(data);
			var left  = until - VipTime.NowTicks();
			return left > 0 ? (int)TimeSpan.FromTicks(left).TotalSeconds : 0;
		}

		public override string ReadCurrentValueAsString(IPlayerDataInfo data)
			=> GetRemainingSeconds(data).ToString();
	}
}