using System.Collections;
using UnityEngine;

namespace Shop
{
	public sealed class Backend
	{
		private const float DEFAULT_DELAY_SECONDS = 3f;

		public bool IsBusy { get; private set; }

		public IEnumerator SendRequestRoutine()
		{
			if (IsBusy) yield break;

			IsBusy = true;
			yield return new WaitForSecondsRealtime(DEFAULT_DELAY_SECONDS);
			IsBusy = false;
		}
	}
}