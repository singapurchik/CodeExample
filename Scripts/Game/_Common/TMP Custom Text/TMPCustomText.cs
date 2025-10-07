using UnityEngine;
using System;

namespace FAS
{
	[Serializable]
	public struct TMPCustomText
	{
		public string Text;
		public Color Color;
		public bool Strikethrough;
		public bool Underline;
		public bool Italic;
		public bool Bold;
	}
}