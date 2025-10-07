using System.Collections.Generic;
using UnityEngine;

namespace FAS.Players.Animations
{
	public class BaseLayer : Layer, IReadOnlyBaseLayer
	{
        private readonly int _healthBehaviourTypeValue = Animator.StringToHash("healthBehaviourType");
        private readonly int _locomotionValueHash = Animator.StringToHash("locomotion");
        private readonly int _isCrouchedBoolHash = Animator.StringToHash("isCrouched");

        public BaseLayer(Animator animator, int index, List<Layer> layersList) : base(animator, index, layersList)
        {
        }

        public float HealthBehaviourTypeValue { get; private set; }
        public float LocomotionValue { get; private set; }
        

        public void SetIsCrouchedState(bool isCrouched) => Animator.SetBool(_isCrouchedBoolHash, isCrouched);
        
        public bool GetIsCrouchedState() => Animator.GetBool(_isCrouchedBoolHash);
	        
        public void SetHealthBehaviourTypeValue(float value)
        {
	        HealthBehaviourTypeValue = value;
	        Animator.SetFloat(_healthBehaviourTypeValue, HealthBehaviourTypeValue);
        }

        public void SetLocomotionValue(float value)
        {
	        LocomotionValue = value;
	        Animator.SetFloat(_locomotionValueHash, LocomotionValue);
        }
	}
}