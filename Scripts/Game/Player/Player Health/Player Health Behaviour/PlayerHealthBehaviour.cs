using System.Collections.Generic;
using FAS.Players.Animations;
using UnityEngine;
using VInspector;
using FAS.Items;
using Zenject;

namespace FAS.Players
{
	public class PlayerHealthBehaviour : MonoBehaviour
	{
		[SerializeField] private PlayerHealthBehaviourData[] _data;
		
		[Inject] private PlayerSoundEffects _soundEffects;
		[Inject] private IItemDropBonus _itemDropBonus;
		[Inject] private PlayerAnimator _animator;
		[Inject] private IReadOnlyHealth _health;
		[Inject] private IPlayerHealthView _view;

		private readonly Dictionary<PlayerHealthBehaviourType, PlayerHealthBehaviourData> _dataDictionary = new (5);
		private PlayerHealthBehaviourData _currentData;

		private void Awake()
		{
			foreach (var data in _data)
				_dataDictionary.TryAdd(data.Type, data);
		}

		private void OnEnable()
		{
			_view.OnVideoLoopPointReached += _soundEffects.PlayHeartPulseSound;
			_health.OnHealthModified += UpdateBehaviour;
			_view.AddOnStartHideListener(OnHideView);
			_view.AddOnStartShowListener(OnShowView);
			_health.OnTakeDamage += UpdateBehaviour;
			_health.OnHeal += UpdateBehaviour;
		}

		private void OnDisable()
		{
			_view.OnVideoLoopPointReached -= _soundEffects.PlayHeartPulseSound;
			_health.OnHealthModified -= UpdateBehaviour;
			_view.RemoveOnStartHideListener(OnHideView);
			_view.RemoveOnStartShowListener(OnShowView);
			_health.OnTakeDamage -= UpdateBehaviour;
			_health.OnHeal -= UpdateBehaviour;
		}

		private void Start()
		{
			UpdateBehaviour();
		}

		private void OnHideView()
		{
			_soundEffects.TryStopHeartPulseSound();
		}

		private void OnShowView()
		{
			_soundEffects.PlayHeartPulseSound();
		}

		private void UpdateBehaviour()
		{
			var caution = _dataDictionary[PlayerHealthBehaviourType.Caution];
			var danger = _dataDictionary[PlayerHealthBehaviourType.Danger];

			var healthValue = _health.Value;
			
			var nextData =
				healthValue > caution.HealthThreshold ? _dataDictionary[PlayerHealthBehaviourType.Fine] :
				healthValue > danger.HealthThreshold  ? caution :
				_dataDictionary[PlayerHealthBehaviourType.Danger];

			if (_currentData != nextData)
				ChangeBehaviour(nextData);
		}


		private void ChangeBehaviour(PlayerHealthBehaviourData data)
		{
			_currentData = data;
			_animator.SmoothChangeHealthBehaviorType(_currentData.AnimatorBehaviourTypeIndex);
			_view.ChangeVideoParameters(_currentData.PulseColor, _currentData.PulseSpeed);
			_itemDropBonus.SetAdditiveBonus(ItemType.InjectorPen, _currentData.InjectorPenDropBonusNormalized);
		}

#if UNITY_EDITOR
		[Button]
		private void ForceChangeData(PlayerHealthBehaviourData data) => ChangeBehaviour(data);
#endif
	}
}