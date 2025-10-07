using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerCostumeChanger : CharacterCostumeChanger<PlayerCostume, IPlayerCostumeData>,
		IPlayerCostumeProxy
	{
		[SerializeField] private CharacterCostumeType _startCostume;

		[Inject] private IReadOnlyInputEvents _inputEvents;
				
		protected override void Awake()
		{
			base.Awake();

			switch (_startCostume)
			{
				case CharacterCostumeType.Girl:
					CurrentCostume = CasualGirl;
					break;
				case CharacterCostumeType.Guy:
				default:
					CurrentCostume = CasualGuy;
					break;
			}
		}

		private void OnEnable()
		{
			_inputEvents.OnSetGirlCostumeButtonClicked += TrySetGirlCostume;
			_inputEvents.OnSetGuyCostumeButtonClicked += TrySetGuyCostume;
		}

		private void OnDisable()
		{
			_inputEvents.OnSetGirlCostumeButtonClicked -= TrySetGirlCostume;
			_inputEvents.OnSetGuyCostumeButtonClicked -= TrySetGuyCostume;
		}

		private void Start()
		{
			ChangeCostume(_startCostume);
		}
	}
}
