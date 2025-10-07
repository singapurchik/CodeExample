using UnityEngine.Events;
using UnityEngine;

namespace FAS
{
	public class SimpleAnimatedLootContainer : LootContainer
	{
		[SerializeField] private Animator _animator;
		[SerializeField] private string _animationName;

		private bool _isForceLootedRequested;
		
		private float _nextUpdateTime;
		
		private int AnimHash => Animator.StringToHash(_animationName);
		
		private const float UPDATE_INTERVAL = 0.25f;
		
		public UnityEvent OnAnimComplete;
		
		public override void Open()
		{
			base.Open();	
			_isForceLootedRequested = false;
			_nextUpdateTime = Time.timeSinceLevelLoad + UPDATE_INTERVAL;
			_animator.Play(AnimHash);
			IsAnimationPlaying = true;
		}
		public void RequestSetLooted()
		{
			_isForceLootedRequested = true;
			IsAnimationPlaying = false;
		}

		public override void Restore()
		{
			_animator.Rebind();
			base.Restore();
		}

		private void Update()
		{
			if (_isForceLootedRequested)
			{
				_animator.Play(AnimHash, 0, 1f);
				_isForceLootedRequested = false;
				OnLootedBefore?.Invoke();
			}
			else if (IsAnimationPlaying && Time.timeSinceLevelLoad > _nextUpdateTime)
			{
				_nextUpdateTime = Time.timeSinceLevelLoad + UPDATE_INTERVAL;

				if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
				{
					InvokeOnLooted();
					IsAnimationPlaying = false;
					OnAnimComplete?.Invoke();
				}
			}
		}
	}
}