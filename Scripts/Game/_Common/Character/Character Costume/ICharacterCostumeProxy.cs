using System;

namespace FAS
{
	public interface ICharacterCostumeProxy<out TInterface> where TInterface : ICharacterCostumeData
	{
		public TInterface Data { get; }
		
		public event Action<TInterface> OnCostumeChanged;
	}
}