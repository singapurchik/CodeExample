using System.Collections.Generic;
using UnityEngine;
using FAS.Utils;
using System;

namespace FAS
{
	public abstract class ScenarioWithSetActiveObjects<TEnum> : Scenario<TEnum> where TEnum : struct, Enum
	{
		[SerializeField] protected List<GameObject> DisableObjectsOnAssemble;
		[SerializeField] protected List<GameObject> EnableObjectsOnAssemble;
		
		protected override void Assemble()
		{
			foreach (var obj in EnableObjectsOnAssemble)
				obj.TryEnable();
			
			foreach (var obj in DisableObjectsOnAssemble)
				obj.TryDisable();
			
			base.Assemble();
		}

		protected override void Disassemble()
		{
			foreach (var obj in EnableObjectsOnAssemble)
				obj.TryDisable();
			
			foreach (var obj in DisableObjectsOnAssemble)
				obj.TryEnable();
			
			base.Disassemble();
		}
	}
}