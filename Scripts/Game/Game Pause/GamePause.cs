using System.Collections.Generic;

namespace FAS
{
	public static class GamePause
	{
		private static readonly HashSet<Pausable> _pausables = new (10);
		
		public static bool IsPaused { get; private set; }

		public static void Add(Pausable pausable)
		{
			_pausables.Add(pausable);
		}
		
		public static void TryPause()
		{
			if (!IsPaused)
			{
				IsPaused = true;

				foreach (var pausable in _pausables)
					pausable.Pause();
			}
		}

		public static void TryPause(IPausableInfo pausableToIgnore)
		{
			if (!IsPaused)
			{
				IsPaused = true;

				foreach (var pausable in _pausables)
				{
					if (pausableToIgnore is Pausable ignored && pausable == ignored) continue;
					pausable.Pause();
				}
			}
		}

		public static void TryPause(Pausable[] pausablesToIgnore)
		{
			if (!IsPaused)
			{
				IsPaused = true;

				int ignoreCount = pausablesToIgnore != null ? pausablesToIgnore.Length : 0;

				foreach (var pausable in _pausables)
				{
					bool isIgnored = false;

					for (int i = 0; i < ignoreCount; i++)
					{
						if (pausable == pausablesToIgnore[i])
						{
							isIgnored = true;
							break;
						}
					}

					if (!isIgnored)
						pausable.Pause();
				}
			}
		}
		
		public static void TryPlay()
		{
			if (IsPaused)
			{
				foreach (var pausable in _pausables)
					pausable.Play();

				IsPaused = false;	
			}
		}
	}
}