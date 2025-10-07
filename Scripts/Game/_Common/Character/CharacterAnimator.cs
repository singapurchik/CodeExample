using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FAS
{
	public class CharacterAnimator : MonoBehaviour, IAnimatorDataChanger, IPausable
	{
		[InjectOptional] private List<ILayerWithTriggers> _layersWithTriggers;
		[Inject] private List<Layer> _layers;
		[Inject] protected Animator Animator;

		private CharacterAnimatorSaver _saver;
		
		private float _speedBeforePause = 1;
		
		protected bool IsForceUpdateThisFrame;

		protected virtual void Awake()
		{
			_saver = new CharacterAnimatorSaver(Animator);
		}

		void IAnimatorDataSaver.SaveData()
		{
			_saver ??= new CharacterAnimatorSaver(Animator);
			_saver.SaveData(_layers);
		}

		void IAnimatorDataSaver.LoadData()
		{
			_saver ??= new CharacterAnimatorSaver(Animator);
			_saver.LoadData(_layers);
			IsForceUpdateThisFrame = true;
		}

		void IAnimatorDataChanger.ChangeData(AnimatorData data)
		{
			Animator.runtimeAnimatorController = data.AnimatorOverrideController;
			Animator.avatar = data.AnimatorAvatar;

			Animator.Rebind();
			Animator.Update(0f);
		}

		void IPausable.Pause()
		{
			_speedBeforePause = Animator.speed;
			Animator.speed = 0;
		}

		void IPausable.Play()
		{
			Animator.speed = _speedBeforePause;
		}
		
		public void TryResetTriggers()
		{
			if (_layersWithTriggers != null)
				foreach (var layer in _layersWithTriggers)
					layer.ResetTriggers();
		}
	}
}