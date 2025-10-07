using System.Collections.Generic;
using UnityEngine;

namespace FAS.Actors.Companion.Animations
{
	public class UniqueStateLayer : LayerWithTriggers, IReadOnlyUniqueStateLayer
	{
		private readonly int _standUpFromBedTriggerHash = Animator.StringToHash("standUpFromBed");

		public int StandUpFromBedAnimHash { get; } = Animator.StringToHash("Stand Up From Bed");
		public int ChillingOnBedAnimHash { get; } = Animator.StringToHash("Chilling On Bed");
		
		public UniqueStateLayer(Animator animator, int index, List<Layer> list, List<ILayerWithTriggers> triggersLit) : base(animator, index, list, triggersLit)
		{
		}
		
		public void PlayStandUpFromBedAnim() => Animator.SetTrigger(_standUpFromBedTriggerHash);
		
		public void PlayChillingOnBedAnim() => PlayAnim(ChillingOnBedAnimHash);

		public override void ResetTriggers()
		{
			Animator.ResetTrigger(_standUpFromBedTriggerHash);
		}
	}
}