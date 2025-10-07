using System;

namespace FAS
{
	public interface IStateMachineInfo<TEnum> where TEnum : struct, Enum
	{
		public TEnum CurrentStateKey { get; }
		public TEnum LastStateKey { get; }
		
		public bool TryGetLastStateKey(out TEnum key);
	}
}