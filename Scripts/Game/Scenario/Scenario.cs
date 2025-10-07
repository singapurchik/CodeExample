using UnityEngine;
using System;

namespace FAS
{
	public abstract class Scenario<TEnum> : MonoBehaviour
		where TEnum : struct, Enum
	{
		public bool IsActive { get; protected set; } = true;
		
		public abstract TEnum Type { get; }
		
		public void TryDisassemble()
		{
			if (IsActive)
				Disassemble();
		}

		public void TryAssemble()
		{
			if (!IsActive)
				Assemble();
		}

		protected virtual void Assemble()
		{
			IsActive = true;
		}

		protected virtual void Disassemble()
		{
			IsActive = false;
		}
	}
}