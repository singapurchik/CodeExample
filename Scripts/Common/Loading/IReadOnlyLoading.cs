using System;

namespace FAS
{
	public interface IReadOnlyLoading
	{
		public bool IsLoading { get; }
		
		public event Action OnLoadComplete;
	}
}