using UnityEngine;
using System;

namespace Core
{
	[Serializable]
	public class IntAmountParameter : IOperationParameter, ISerializationCallbackReceiver
	{
		[Min(1)] public int Amount = 1;

		public void OnBeforeSerialize() { if (Amount < 1) Amount = 1; }

		public void OnAfterDeserialize() { if (Amount < 1) Amount = 1; }
	}
}