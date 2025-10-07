using UnityEngine;
using VInspector;
using FAS.Utils;
using Zenject;
using System;

namespace FAS
{
	public abstract class CharacterCostumeChanger<TCostume, TReadOnlyCostumeData> : MonoBehaviour
		where TReadOnlyCostumeData : class, ICharacterCostumeData
		where TCostume : CharacterCostume, TReadOnlyCostumeData
	{
		[SerializeField] protected TCostume CasualGirl;
		[SerializeField] protected TCostume CasualGuy;
		
		[Inject] private IAnimatorDataChanger _animatorDataChanger;
		
		protected TCostume CurrentCostume;
		protected TCostume[] Costumes;

		public TReadOnlyCostumeData Data => CurrentCostume;

		public event Action<TReadOnlyCostumeData> OnCostumeChanged;
		
		protected virtual void Awake()
		{
			Costumes = new[]
			{
				CasualGirl,
				CasualGuy
			};
		}

		public void TrySetGirlCostume() => TryChangeCostume(CharacterCostumeType.Girl);
		
		public void TrySetGuyCostume() => TryChangeCostume(CharacterCostumeType.Guy);

		public virtual bool TryChangeCostume(CharacterCostumeType costumeType, bool isSavingAnimatorStates = true)
		{
			if (CurrentCostume == null || Data.Type != costumeType)
			{
				ChangeCostume(costumeType, isSavingAnimatorStates);
				return true;
			}

			return false;
		}
		
		protected void ChangeCostume(CharacterCostumeType costumeType, bool isSavingAnimatorStates = true)
		{
			foreach (var costume in Costumes)
			{
				if (costume.Type == costumeType)
				{
					CurrentCostume = costume;
					costume.gameObject.TryEnable();
				}
				else
				{
					costume.gameObject.TryDisable();
				}
			}

			if (isSavingAnimatorStates)
				_animatorDataChanger.SaveData();
			
			_animatorDataChanger.ChangeData(Data.AnimatorData);
			_animatorDataChanger.LoadData();
			OnCostumeChanged?.Invoke(Data);
		}

#if UNITY_EDITOR
		[Button]
		private void ChangeToGirl() => TrySetGirlCostume();

		[Button]
		private void ChangeToGuy() => TrySetGuyCostume();
#endif
	}
}
