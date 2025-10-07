using System.Collections.Generic;
using UnityEngine;

namespace FAS.Players.Animations
{
	public class RightHandLayer : LayerWithTriggers, IReadOnlyRightHandLayer
	{
		private readonly int _rightHandAnimTypeValueHash = Animator.StringToHash("rightHandAnimType");
		private readonly int _useRightHandTriggerHash = Animator.StringToHash("useRightHand");


		public RightHandLayer(Animator animator, int index, List<Layer> list, List<ILayerWithTriggers> triggersLit)
			: base(animator, index, list, triggersLit)
		{
		}

		public int PickUpShortAnimHash => Animator.StringToHash("PickUp Short");
		public int PickUpFullAnimHash => Animator.StringToHash("PickUp Full");
		public int InteractAnimHash => Animator.StringToHash("Interact");
		
		private const int INTERACT_INDEX = 0;
		private const int PICK_UP_INDEX = 1;
		

		public void PlayInteractAnim() => PlayAnim(INTERACT_INDEX);
		
		public void PlayPickUpAnim() => PlayAnim(PICK_UP_INDEX);

		private void PlayAnim(int index)
		{
			Animator.SetInteger(_rightHandAnimTypeValueHash, index);
			Animator.SetTrigger(_useRightHandTriggerHash);
		}

		public override void ResetTriggers()
		{
			Animator.ResetTrigger(_useRightHandTriggerHash);
		}
	}
}