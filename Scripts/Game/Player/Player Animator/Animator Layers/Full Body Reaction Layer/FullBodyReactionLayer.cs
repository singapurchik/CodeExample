using System.Collections.Generic;
using UnityEngine;

namespace FAS.Players.Animations
{
	public class FullBodyReactionLayer : Layer, IReadOnlyFullBodyReactionLayer
	{
		public int KnockedDownAnimHash => Animator.StringToHash("Knocked Down");
        public int FlyingBackAnimHash => Animator.StringToHash("Flying Back");
        public int EdgeSlipAnimHash => Animator.StringToHash("Edge Slip");
        public int StandUpAnimHash => Animator.StringToHash("Stand Up");
        
        public FullBodyReactionLayer(Animator animator, int index, List<Layer> layersList)
	        : base(animator, index, layersList)
        {
        }

        public void PlayFromSlipToFlyingBackAnim() => PlayAnim(EdgeSlipAnimHash);
        
        public void PlayKnockedDownAnim() => PlayAnim(KnockedDownAnimHash);
	}
}