using System.Collections.Generic;
using UnityEngine;

namespace FAS.Actors.Emenies.Animations
{
	public class BaseLayer : Layer
	{
		private readonly int _locomotionValueHash = Animator.StringToHash("locomotion");

		public float LocomotionValue { get; private set; }

		public BaseLayer(Animator animator, int index, List<Layer> layersList) : base(animator, index, layersList)
		{
		}
		
		public void SetLocomotionValue(float value)
		{
			LocomotionValue = value;
			Animator.SetFloat(_locomotionValueHash, LocomotionValue);
		}
	}
}