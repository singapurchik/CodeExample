using System.Collections.Generic;
using UnityEngine;

namespace FAS.Actors.Companion.Animations
{
	public class LowerBodyLayer : LayerWithTriggers
	{
		private readonly int _turnInPlaceTypeValueHash = Animator.StringToHash("turnInPlaceType");
		private readonly int _turnInPlaceTriggerHash = Animator.StringToHash("turnInPlace");

		private int _currentTurnInPlaceTypeValue;

		protected override float MaxLayerWeight { get; } = 0.7f;
		protected override float DisableSpeed { get; } = 6f;
		protected override float EnableSpeed { get; } = 3f;
		
		public LowerBodyLayer(Animator animator, int index, List<Layer> list, List<ILayerWithTriggers> triggersLit)
			: base(animator, index, list, triggersLit)
		{
		}

		public void PlayTurnInPlaceAnim(int type)
		{
			if (_currentTurnInPlaceTypeValue != type)
			{
				_currentTurnInPlaceTypeValue =  type;
				Animator.SetInteger(_turnInPlaceTypeValueHash, type);
			}
			
			Animator.SetTrigger(_turnInPlaceTriggerHash);
		}

		public override void ResetTriggers()
		{
			Animator.ResetTrigger(_turnInPlaceTriggerHash);
		}
	}
}