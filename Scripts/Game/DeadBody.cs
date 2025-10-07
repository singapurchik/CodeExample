using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;

namespace FAS
{
	[RequireComponent(typeof(Animator))]
	public class DeadBody : MonoBehaviour, IInteractableVisitable
	{
		[SerializeField] private UnityEvent _onWakeUpAnimComplete;
		
		private Animator _animator;
		
		private readonly int _wakeUpTriggerHash = Animator.StringToHash("wakeUp");
		private readonly int _wakeUpAnimHash = Animator.StringToHash("Wake Up");

		private bool _isSleeping = true;
		private bool _isActive;
		
		public bool IsWakeUpAnimPlaying { get; private set; }
		public float AnimNormalizedTime { get; private set; }

		private const int ANIMATOR_ADDITIVE_LAYER_INDEX = 1;

		private void Awake()
		{
			_animator = GetComponent<Animator>();
		}

		private void OnEnable()
		{
			if (!_isSleeping)
				_animator.SetLayerWeight(ANIMATOR_ADDITIVE_LAYER_INDEX, 0);
		}

		public void Accept(IInteractableVisitor visitor) => visitor.Apply(this);
		
		public void PlayWakeUpAnim() => StartCoroutine(PlayWakeUpAnimRoutine());

		private IEnumerator PlayWakeUpAnimRoutine()
		{
			_isSleeping = false;
			_animator.SetTrigger(_wakeUpTriggerHash);
			_animator.SetLayerWeight(ANIMATOR_ADDITIVE_LAYER_INDEX, 0);
			yield return null;
			_isActive = true;
		}

		private void Update()
		{
			if (_isActive)
			{
				var info = _animator.GetCurrentAnimatorStateInfo(0);
				
				AnimNormalizedTime = info.normalizedTime;
				IsWakeUpAnimPlaying = info.shortNameHash == _wakeUpAnimHash;

				if (IsWakeUpAnimPlaying && info.normalizedTime > 0.99f)
				{
					_onWakeUpAnimComplete?.Invoke();
					_isActive = false;
				}
			}
		}
	}
}