using System.Collections.Generic;
using UnityEngine;

namespace FAS
{
    public class Layer : IReadOnlyAnimatorLayer
    {
	    private struct LayerSnapshot
	    {
		    public int FromHash;
		    public float FromNTime;

		    public int ToHash;
		    public float ToNTime;

		    public bool WasInTransition;
		    public float LayerWeight;
	    }
	
	    private readonly struct AnimatorStateData
	    {
		    public int CurrentStateHash { get; }
		    public int NextStateHash { get; }

		    public float NormalizedTime { get; }
		
		    public bool IsActive => CurrentStateHash != EmptyAnimHash;
		    public bool IsInTransition { get; }

		    public AnimatorStateData(AnimatorStateInfo currentInfo, AnimatorStateInfo nextInfo, bool isInTransition)
		    {
			    NormalizedTime = currentInfo.normalizedTime;
			    CurrentStateHash = currentInfo.shortNameHash;
			    NextStateHash = nextInfo.shortNameHash;
			    IsInTransition = isInTransition;
		    }
	    }
	    
        protected readonly Animator Animator;
        private LayerSnapshot _savedData;
        private AnimatorStateData _data;

        protected readonly int Index;
        
        private int _lastCachedFrame = -1;

        protected virtual float MaxLayerWeight { get; } = 1f;
        protected virtual float DisableSpeed { get; } = 10f;
        protected virtual float EnableSpeed { get; } = 10f;

        public float CurrentAnimNTime => GetStateData().NormalizedTime;
        public int CurrentAnimHash => GetStateData().CurrentStateHash;
        public int NextAnimHash => GetStateData().NextStateHash;
        
        public float Weight { get; private set; }

        public bool IsInTransition => GetStateData().IsInTransition;
        public bool IsEnabled => Weight >= MaxLayerWeight;
        public bool IsActive => GetStateData().IsActive;
        public bool IsHasSavedData { get; private set; }
        public bool IsDisabled => Weight <= 0f;

        public static int EmptyAnimHash => Animator.StringToHash("Empty");

        public Layer(Animator animator, int index, List<Layer> layersList)
        {
            Animator = animator;
            Index = index;
            Weight = Animator.GetLayerWeight(Index);
            layersList.Add(this);
        }

        private void Invalidate() => _lastCachedFrame = -1;

        public void SaveData()
        {
            var currentStateInfo = Animator.GetCurrentAnimatorStateInfo(Index);
            var wasInTransition = Animator.IsInTransition(Index);

            _savedData = new LayerSnapshot
            {
                FromHash = currentStateInfo.shortNameHash,
                FromNTime = currentStateInfo.normalizedTime,
                WasInTransition = wasInTransition,
                LayerWeight = Animator.GetLayerWeight(Index),
                ToHash = 0,
                ToNTime = 0f,
            };

            if (wasInTransition)
            {
                var next = Animator.GetNextAnimatorStateInfo(Index);
                _savedData.ToHash = next.shortNameHash;
                _savedData.ToNTime = next.normalizedTime;
            }

            IsHasSavedData = true;
        }

        public void RestoreData(float crossfadeDuration = 0.06f)
        {
            SetWeight(_savedData.LayerWeight);

            if (_savedData.WasInTransition)
            {
	            PlayAnim(_savedData.FromHash, Mathf.Repeat(_savedData.FromNTime, 1f));
	            Animator.CrossFade(_savedData.ToHash, crossfadeDuration, Index, 
		            Mathf.Repeat(_savedData.ToNTime, 1f));
            }
            else
            {
	            PlayAnim(_savedData.FromHash, Mathf.Repeat(_savedData.FromNTime, 1f));
            }
            
	        Invalidate();
        }

        protected void PlayAnim(int animHash, float nTime = 0f)
            => Animator.Play(animHash, Index, nTime);

        public void EnableWeightSmooth() =>
            SetWeight(Mathf.MoveTowards(Weight, MaxLayerWeight, EnableSpeed * Time.deltaTime));

        public void DisableWeightSmooth() =>
            SetWeight(Mathf.MoveTowards(Weight, 0f, DisableSpeed * Time.deltaTime));

        public void Disable() => SetWeight(0f);
        public void Enable() => SetWeight(MaxLayerWeight);

        private void SetWeight(float weight)
        {
            Weight = weight;
            Animator.SetLayerWeight(Index, Weight);
        }

        private AnimatorStateData GetStateData()
        {
            if (Time.frameCount != _lastCachedFrame)
            {
                _data = new AnimatorStateData(
                    Animator.GetCurrentAnimatorStateInfo(Index),
                    Animator.GetNextAnimatorStateInfo(Index), Animator.IsInTransition(Index));
                
                _lastCachedFrame = Time.frameCount;
            }
            return _data;
        }
    }
}