namespace Core
{
	public interface IPlayerDataInfo
	{
		public string GetString(PlayerDataKey key, string defaultValue = "");
		public bool GetBool(PlayerDataKey key, bool defaultValue = false);
		public float GetFloat(PlayerDataKey key, float defaultValue = 0f);
		public int GetInt(PlayerDataKey key, int defaultValue = 0);
	}
}