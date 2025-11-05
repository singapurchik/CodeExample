#if UNITY_EDITOR
using UnityEditor;
using Core;

namespace Shop.EditorTools
{
	internal static class EntryDrawerUtil
	{
		public const string OperationProp = "Operation";
		public const string ParameterProp = "Parameter";

		public static bool EnsureParamCompatibility(SerializedProperty entryProp)
		{
			var opProp    = entryProp.FindPropertyRelative(OperationProp);
			var paramProp = entryProp.FindPropertyRelative(ParameterProp);

			var opObj = opProp?.objectReferenceValue as PlayerDataOperation;
			if (opObj == null)
			{
				if (paramProp != null && paramProp.managedReferenceValue != null)
				{
					paramProp.managedReferenceValue = null;
					return true;
				}
				return false;
			}

			if (opObj is IOperationWithParameter withParam)
			{
				var current = paramProp?.managedReferenceValue as IOperationParameter;
				if (current == null || !withParam.IsSupports(current))
				{
					var def = withParam.CreateDefaultParam();
					if (paramProp != null)
					{
						paramProp.managedReferenceValue = def;
						return true;
					}
				}
				return false;
			}

			if (paramProp != null && paramProp.managedReferenceValue != null)
			{
				paramProp.managedReferenceValue = null;
				return true;
			}
			return false;
		}
	}
}
#endif