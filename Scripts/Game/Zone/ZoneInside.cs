using System.Collections.Generic;
using UnityEngine;
using FAS.Utils;

namespace FAS
{
	public class ZoneInside : MonoBehaviour
	{
		[SerializeField] private List<GameObject> _enabledObjects;
		
		public virtual void Show()
		{
			foreach (var obj in _enabledObjects)
				obj.TryEnable();
		}
		
		public virtual void Hide()
		{
			foreach (var obj in _enabledObjects)
				obj.TryDisable();
		}
	}
}