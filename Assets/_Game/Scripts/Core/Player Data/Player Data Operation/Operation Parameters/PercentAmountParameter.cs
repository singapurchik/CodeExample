using UnityEngine;
using System;

namespace Core
{
	[Serializable]
	public class PercentAmountParameter : IOperationParameter, ISerializationCallbackReceiver
	{
		[Min(1)] public int Percent = 1;

		public void OnBeforeSerialize() { if (Percent < 1) Percent = 1; }

		public void OnAfterDeserialize() { if (Percent < 1) Percent = 1; }
	}
}