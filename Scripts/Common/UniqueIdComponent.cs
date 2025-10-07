using UnityEditor;
using UnityEngine;
using VInspector;
using System;

namespace FAS
{
	public abstract class UniqueIdComponent : MonoBehaviour
	{
		[ReadOnly] [SerializeField] private string _uniqueId;

		public string UniqueId => _uniqueId;

#if UNITY_EDITOR
		[Button]
		public void ClearId()
		{
			_uniqueId = string.Empty;
		}
		
		protected virtual void Reset()
		{
			if (string.IsNullOrEmpty(_uniqueId))
			{
				_uniqueId = Guid.NewGuid().ToString("N");
				EditorUtility.SetDirty(this);
			}
		}
#endif
	}
}