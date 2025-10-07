using System.Collections.Generic;
using UnityEngine;

namespace FAS.Actors.Companion.Animations
{
	public class BaseLayer : LayerWithTriggers, IReadOnlyBaseLayer
	{
		private readonly int _startLocomotionTriggerHash = Animator.StringToHash("startLocomotion");
		private readonly int _knockedBackTriggerHash = Animator.StringToHash("knockBack");
		private readonly int _locomotionValueHash = Animator.StringToHash("locomotion");

		public BaseLayer(Animator animator, int index, List<Layer> list, List<ILayerWithTriggers> triggersLit)
			: base(animator, index, list, triggersLit)
		{
		}

		public int KnockedBackAnimHash { get; } = Animator.StringToHash("Knocked Back");
		public bool IsIdleAnimPlaying => LocomotionValue == 0;

		public float LocomotionValue { get; private set; }
		

		public void SetLocomotionValue(float value)
		{
			LocomotionValue = value;
			Animator.SetFloat(_locomotionValueHash, LocomotionValue);
		}
		
		public void PlayStartLocomotionAnim() => Animator.SetTrigger(_startLocomotionTriggerHash);
		

		public void PlayKnockedBackAnim() => Animator.SetTrigger(_knockedBackTriggerHash);
		
		public override void ResetTriggers()
		{
			Animator.ResetTrigger(_startLocomotionTriggerHash);
			Animator.ResetTrigger(_knockedBackTriggerHash);
		}
	}
}