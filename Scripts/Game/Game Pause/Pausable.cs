using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FAS
{
	public class Pausable : MonoBehaviour, IPausableInfo
	{
		[Inject] private List<IPausable> _pausables;
		
		public bool IsPaused { get; private set; }
		
		private void Awake() => GamePause.Add(this);

		public void Pause()
		{
			foreach (var pausable in _pausables)
				pausable.Pause();

			IsPaused = true;
		}

		public void Play()
		{
			foreach (var pausable in _pausables)
				pausable.Play();
			
			IsPaused = false;
		}
	}
}