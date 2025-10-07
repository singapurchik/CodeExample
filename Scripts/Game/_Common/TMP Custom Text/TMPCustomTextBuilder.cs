using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FAS
{
	public static class TMPCustomTextBuilder
	{
		public static string Build(IList<TMPCustomText> fragments, char separator = ' ')
		{
			string result;

			if (fragments == null || fragments.Count == 0)
			{
				result = string.Empty;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder(64);

				for (int index = 0; index < fragments.Count; index++)
				{
					if (index > 0)
						stringBuilder.Append(separator);

					AppendFormattedFragment(stringBuilder, fragments[index]);
				}

				result = stringBuilder.ToString();
			}

			return result;
		}

		public static string Build(TMPCustomText fragment)
		{
			StringBuilder stringBuilder = new StringBuilder(32);
			AppendFormattedFragment(stringBuilder, fragment);
			return stringBuilder.ToString();
		}

		private static void AppendFormattedFragment(StringBuilder stringBuilder, TMPCustomText fragment)
		{
			string escapedText = EscapeRichText(fragment.Text ?? string.Empty);
			string colorCode = ColorUtility.ToHtmlStringRGBA(fragment.Color);

			stringBuilder.Append("<color=#").Append(colorCode).Append('>');

			if (fragment.Bold)
				stringBuilder.Append("<b>");
			if (fragment.Italic)
				stringBuilder.Append("<i>");
			if (fragment.Underline)
				stringBuilder.Append("<u>");
			if (fragment.Strikethrough)
				stringBuilder.Append("<s>");

			stringBuilder.Append(escapedText);

			if (fragment.Strikethrough)
				stringBuilder.Append("</s>");
			if (fragment.Underline)
				stringBuilder.Append("</u>");
			if (fragment.Italic)
				stringBuilder.Append("</i>");
			if (fragment.Bold)
				stringBuilder.Append("</b>");

			stringBuilder.Append("</color>");
		}

		private static string EscapeRichText(string sourceText)
		{
			string result;

			if (string.IsNullOrEmpty(sourceText))
			{
				result = string.Empty;
			}
			else
			{
				string temp = sourceText.Replace("&", "&amp;");
				temp = temp.Replace("<", "&lt;");
				temp = temp.Replace(">", "&gt;");
				result = temp;
			}

			return result;
		}
	}
}