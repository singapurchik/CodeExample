using System.Collections.Generic;
using System.Text;
using System;

namespace FAS.Extensions
{
	public static class EnumExtensions
	{
		private static readonly Dictionary<Enum, string> _spacedNameCache = new();

		public static string ToSpacedString(this Enum value)
		{
			if (_spacedNameCache.TryGetValue(value, out var cachedName))
				return cachedName;

			string name = value.ToString();
			var result = new StringBuilder(name.Length * 2);

			for (int i = 0; i < name.Length; i++)
			{
				if (i > 0 && char.IsUpper(name[i]))
					result.Append(' ');

				result.Append(name[i]);
			}

			string finalName = result.ToString();
			_spacedNameCache[value] = finalName;
			return finalName;
		}
	}
}