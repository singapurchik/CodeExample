using UnityEngine;
using System;
using Core;

namespace Shop
{
	[Serializable]
	public class CostEntry
	{
		public ConsumeOperation Operation;
		[SerializeReference] public IOperationParameter Parameter;
	}
}