using System.Collections.Generic;
using UnityEngine;

namespace FAS.Actors.Emenies.Animations
{
	public class FatalityLayer : Layer
	{
		private readonly int _stabStomachAnimHash = Animator.StringToHash("Stab Stomach");
		private readonly int _neckSlashAnimHash = Animator.StringToHash("Neck Slash");

		public FatalityLayer(Animator animator, int index, List<Layer> layersList) : base(animator, index, layersList)
		{
		}

		public void PlayStabStomachAnim() => Animator.Play(_stabStomachAnimHash, Index, 0f);
		
		public void PlayNeckSlashAnim() => Animator.Play(_neckSlashAnimHash, Index, 0f);
	}
}