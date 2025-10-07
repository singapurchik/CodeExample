using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FAS
{
	public abstract class CharacterClothesWetness<TCostumeProxy, TCostumeData> : MonoBehaviour, IClothesWetness
		where TCostumeData : ICharacterCostumeData
		where TCostumeProxy : ICharacterCostumeProxy<TCostumeData>
	{
		[Inject] private List<ClothesWetness> _clothesWetnesses;
		[Inject] private TCostumeProxy _costumeProxy;
		
		private IClothesWetness _currentClothesWetness;
		
		public float CurrentWetAmount { get; private set; }

		private void OnEnable()
		{
			_costumeProxy.OnCostumeChanged += ChangeClothesWetness;
		}

		private void OnDisable()
		{
			_costumeProxy.OnCostumeChanged -= ChangeClothesWetness;
		}

		public void RequestChangeDisableWetnessSpeed(float speed)
			=> _currentClothesWetness.RequestChangeDisableWetnessSpeed(speed);

		public void RequestEnableWetnessSmooth(float speed = 0)
			=> _currentClothesWetness.RequestEnableWetnessSmooth(speed);

		public void RequestForceEnableWetness()
			=> _currentClothesWetness.RequestForceEnableWetness();

		private void ChangeClothesWetness(TCostumeData data)
		{
			foreach (var clothesWetness in _clothesWetnesses)
				clothesWetness.ForceChangeWetAmount(CurrentWetAmount);
			
			_currentClothesWetness = data.ClothesWetness;
		}

		private void LateUpdate()
		{
			CurrentWetAmount = _currentClothesWetness.CurrentWetAmount;
		}
	}
}