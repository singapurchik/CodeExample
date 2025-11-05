using System;
using Core;

namespace VIP
{
	internal static class VipTime
	{
		public static long SecondsToTicks(int seconds) => Math.Max(1, seconds) * TICKS_PER_SECOND;
		public static long NowTicks() => DateTime.UtcNow.Ticks;

		private const long TICKS_PER_SECOND = TimeSpan.TicksPerSecond;

		public static bool TryReadUntil(IPlayerDataInfo data, PlayerDataKey key, out long until)
		{
			var s = data.GetString(key, "0");
			if (long.TryParse(s, out var t)) { until = t; return true; }
			until = 0;
			return false;
		}

		public static void WriteUntil(PlayerData data, PlayerDataKey key, long ticks)
			=> data.SetString(key, ticks <= 0 ? "0" : ticks.ToString());
	}
}