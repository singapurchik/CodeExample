using System.Collections.Generic;

namespace Core
{
	public sealed class PlayerData : IPlayerDataInfo
	{
		private readonly Dictionary<string, string> _strings = new(64);
		private readonly Dictionary<string, float> _floats = new(64);
		private readonly Dictionary<string, bool> _bools = new(64);
		private readonly Dictionary<string, int> _ints = new(64);

		public int GetInt(PlayerDataKey key, int defaultValue = 0)
			=> _ints.GetValueOrDefault(key.Id, defaultValue);

		public void SetInt(PlayerDataKey key, int value) => _ints[key.Id] = value;

		public float GetFloat(PlayerDataKey key, float defaultValue = 0f)
			=> _floats.GetValueOrDefault(key.Id, defaultValue);

		public void SetFloat(PlayerDataKey key, float value) => _floats[key.Id] = value;

		public bool GetBool(PlayerDataKey key, bool defaultValue = false)
			=> _bools.TryGetValue(key.Id, out var v) && v || defaultValue;

		public void SetBool(PlayerDataKey key, bool value) => _bools[key.Id] = value;

		public string GetString(PlayerDataKey key, string defaultValue = "")
			=> _strings.GetValueOrDefault(key.Id, defaultValue);

		public void SetString(PlayerDataKey key, string value) => _strings[key.Id] = value;
	}
}