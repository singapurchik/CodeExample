using System.Collections.Generic;
using FAS.Actors.Emenies;
using System;

namespace FAS.Players
{
	public class PlayerPursuersHolder : IPlayerPursuersHolder
	{
		private readonly HashSet<IPursuiter> _pursuers = new (10);

		public event Action OnLastPursuiterRemoved;
		public event Action OnFirstPursuiterAdded;
		
		public void TryRemove(IPursuiter pursuiter)
		{
			_pursuers.Remove(pursuiter);
			
			if (_pursuers.Count == 0)
				OnLastPursuiterRemoved?.Invoke();
		}

		public void TryAdd(IPursuiter pursuiter)
		{
			if (_pursuers.Count == 0)
				OnFirstPursuiterAdded?.Invoke();
			
			_pursuers.Add(pursuiter);
		}

		public void Clear()
		{
			if (_pursuers.Count == 0)
				OnLastPursuiterRemoved?.Invoke();
			
			_pursuers.Clear();
		}
	}
}