using System.Collections.Generic;
using UnityEngine;

namespace FAS.Actors.Emenies.Animations
{
	public class UpperBodyAdditiveLayer : LayerWithTriggers, IReadOnlyUpperBodyAdditiveLayer
	{
		private readonly int _idleTriggerHash = Animator.StringToHash("idleAdditive");
		private readonly int _walkTriggerHash = Animator.StringToHash("walkAdditive");
		private readonly int _runTriggerHash = Animator.StringToHash("runAdditive");

		public int IdleAnimHash { get; } = Animator.StringToHash("Idle");
		public int WalkAnimHash { get; } = Animator.StringToHash("Walk");
		public int RunAnimHash { get; } = Animator.StringToHash("Run");
		
		public UpperBodyAdditiveLayer(Animator animator, int index, List<Layer> list,
			List<ILayerWithTriggers> triggersLit) : base(animator, index, list, triggersLit)
		{
		}

		public void PlayIdleAnim() => Animator.SetTrigger(_idleTriggerHash);
		
		public void ResetIdleAnim() => Animator.ResetTrigger(_idleTriggerHash);
		
		public void PlayWalkAnim() => Animator.SetTrigger(_walkTriggerHash);
		
		public void ResetWalkAnim() => Animator.ResetTrigger(_walkTriggerHash);
		
		public void PlayRunAnim() => Animator.SetTrigger(_runTriggerHash);
		
		public void ResetRunAnim() => Animator.ResetTrigger(_runTriggerHash);
		
		public override void ResetTriggers()
		{
			Animator.ResetTrigger(_idleTriggerHash);
			Animator.ResetTrigger(_walkTriggerHash);
			Animator.ResetTrigger(_runTriggerHash);
		}
	}
}