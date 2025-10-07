using UnityEngine;

namespace FAS.Utils
{
	public static class GameObjectExtensions
	{
		public static bool TryDisable(this GameObject obj)
		{
			if (obj.activeSelf)
			{
				obj.SetActive(false);
				return true;
			}
			return false;
		}

		public static bool TryEnable(this GameObject obj)
		{
			if (!obj.activeSelf)
			{
				obj.SetActive(true);
				return true;
			}
			return false;
		}
	}
}