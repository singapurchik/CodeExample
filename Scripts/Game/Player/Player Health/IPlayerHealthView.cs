using UnityEngine;
using System;
using FAS.UI;

namespace FAS.Players
{
	public interface IPlayerHealthView : IReadOnlyUIScreen
	{
		public event Action OnVideoLoopPointReached;
		
		public void ChangeVideoParameters(Color color, float speed);
	}
}