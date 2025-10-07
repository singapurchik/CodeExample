using System.Collections.Generic;
using UnityEngine;

namespace FAS.Actors.Emenies.Animations
{
	public class AttackLayer : Layer
	{
		private readonly int _attackAnimHash = Animator.StringToHash("Attack");

		public AttackLayer(Animator animator, int index, List<Layer> layersList) : base(animator, index, layersList)
		{
		}

		public void PlayAttackAnim() => Animator.Play(_attackAnimHash, Index, 0f);
	}
}