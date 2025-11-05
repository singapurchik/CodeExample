using System.Collections.Generic;
using System.Text;
using Core;

namespace Shop
{
	public sealed class BundlePhraseFormatter
	{
		private const string FOR_SEPARATOR = " for ";
		private const string COMMA_SEPARATOR = ", ";
		private const string AND_WORD = " and ";

		private readonly StringBuilder _stringBuilder = new(64);

		public string Build(IReadOnlyList<IPlayerDataOperationInfo> costs,
			IReadOnlyList<IPlayerDataOperationInfo> rewards)
		{
			_stringBuilder.Clear();

			var rewardsCount = CountValid(rewards);
			var costsCount = CountValid(costs);

			if (rewardsCount > 0)
				AppendJoinedWithAnd(_stringBuilder, rewards, rewardsCount);

			if (rewardsCount > 0 && costsCount > 0)
				_stringBuilder.Append(FOR_SEPARATOR);

			if (costsCount > 0)
				AppendJoinedWithAnd(_stringBuilder, costs, costsCount);

			NormalizeCasing(_stringBuilder);
			return _stringBuilder.ToString();
		}

		private int CountValid(IReadOnlyList<IPlayerDataOperationInfo> list)
		{
			if (list != null)
			{
				var count = 0;
				for (int i = 0; i < list.Count; i++)
				{
					var info = list[i]?.Info;
					
					if (info != null && !string.IsNullOrEmpty(info.DisplayName))
						count++;
				}
				return count;
			}
			return 0;
		}

		private void AppendJoinedWithAnd(StringBuilder stringBuilder,
			IReadOnlyList<IPlayerDataOperationInfo> list, int totalValid)
		{
			int appended = 0;

			for (int i = 0; i < list.Count; i++)
			{
				var name = list[i]?.Info?.DisplayName;

				if (!string.IsNullOrEmpty(name))
				{
					if (appended > 0)
						stringBuilder.Append(appended == totalValid - 1 ? AND_WORD : COMMA_SEPARATOR);

					stringBuilder.Append(name);
					appended++;
					if (appended == totalValid) break;
				}
			}
		}

		private void NormalizeCasing(StringBuilder stringBuilder)
		{
			if (stringBuilder.Length > 0)
			{
				int first = 0;

				while (first < stringBuilder.Length && char.IsWhiteSpace(stringBuilder[first]))
					first++;

				if (first < stringBuilder.Length)
				{
					stringBuilder[first] = char.ToUpperInvariant(stringBuilder[first]);

					for (int i = first + 1; i < stringBuilder.Length; i++)
						stringBuilder[i] = char.ToLowerInvariant(stringBuilder[i]);
				}
			}
		}
	}
}