using System.Collections.Generic;
using UnityEngine;

namespace FAS.Players.Animations
{
	public class UpperBodyAdditiveLayer : Layer
	{
		public int TakeDamageAnimHash => Animator.StringToHash("Take Damage");

		public UpperBodyAdditiveLayer(Animator animator, int index, List<Layer> layersList)
			: base(animator, index, layersList)
		{
		}
		
		public void PlayTakeDamageAnim() => Animator.Play(TakeDamageAnimHash, Index, 0f);
	}
}