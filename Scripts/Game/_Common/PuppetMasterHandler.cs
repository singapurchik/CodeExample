using RootMotion.Dynamics;
using UnityEngine;
using VInspector;
using System;

namespace FAS
{
	public class PuppetMasterHandler : MonoBehaviour
	{
		[SerializeField] private PuppetMaster _puppetMaster;
		[SerializeField] private BehaviourPuppet _behaviourPuppet;
		
		public Transform PuppetTransform => _puppetMaster.transform;
		
		public PuppetMaster.Mode CurrentMode => _puppetMaster.mode;
		public Muscle[] Muscles => _puppetMaster.muscles;
		
		public event Action OnRegainBalance;
		public event Action OnLoseBalance;
		public event Action OnTryGetUp;

		private void OnEnable()
		{
			_behaviourPuppet.onLoseBalanceFromPuppet.unityEvent.AddListener(InvokeOnLoseBalance);
			_behaviourPuppet.onLoseBalanceFromGetUp.unityEvent.AddListener(InvokeOnLoseBalance);
			_behaviourPuppet.onRegainBalance.unityEvent.AddListener(InvokeOnRegainBalance);
			_behaviourPuppet.onLoseBalance.unityEvent.AddListener(InvokeOnLoseBalance);
			_behaviourPuppet.onGetUpSupine.unityEvent.AddListener(InvokeOnTryGetUp);
			_behaviourPuppet.onGetUpProne.unityEvent.AddListener(InvokeOnTryGetUp);
		}
		
		private void OnDisable()
		{
			_behaviourPuppet.onLoseBalanceFromPuppet.unityEvent.RemoveListener(InvokeOnLoseBalance);
			_behaviourPuppet.onLoseBalanceFromGetUp.unityEvent.RemoveListener(InvokeOnLoseBalance);
			_behaviourPuppet.onRegainBalance.unityEvent.RemoveListener(InvokeOnRegainBalance);
			_behaviourPuppet.onLoseBalance.unityEvent.RemoveListener(InvokeOnLoseBalance);
			_behaviourPuppet.onGetUpSupine.unityEvent.RemoveListener(InvokeOnTryGetUp);
			_behaviourPuppet.onGetUpProne.unityEvent.RemoveListener(InvokeOnTryGetUp);
		}

		public Muscle GetMuscle(int index) => Muscles[index];
		
		public void DisableGetUpLogic() => _behaviourPuppet.canGetUp = false;
		
		public void SetPinWeight(float weight) => _puppetMaster.pinWeight = weight;
		
		public void SetMode(PuppetMaster.Mode mode) => _puppetMaster.mode = mode;
		
		public void SetState(PuppetMaster.State state) => _puppetMaster.state = state;
		
		public void SetBehaviourState(BehaviourPuppet.State state) => _behaviourPuppet.SetState(state);

		private void InvokeOnRegainBalance() => OnRegainBalance?.Invoke();
		
		private void InvokeOnLoseBalance() => OnLoseBalance?.Invoke();
		
		private void InvokeOnTryGetUp() => OnTryGetUp?.Invoke();
		
		public void Restore()
		{
			SetBehaviourState(BehaviourPuppet.State.Puppet);
			SetState(PuppetMaster.State.Alive);
			SetMode(PuppetMaster.Mode.Active);
			SetPinWeight(1);
			_behaviourPuppet.canGetUp = true;
		}

#if UNITY_EDITOR
		[Button]
		private void FindBehaviours()
		{
			_behaviourPuppet = GetComponentInChildren<BehaviourPuppet>(true);
			_puppetMaster = GetComponentInChildren<PuppetMaster>(true);
		}
#endif
	}
}