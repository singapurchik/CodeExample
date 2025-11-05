using UnityEngine;
using System;
using Core;

[Serializable]
public class RewardEntry
{
	public ProvideOperation Operation;
	[SerializeReference] public IOperationParameter Parameter;
}