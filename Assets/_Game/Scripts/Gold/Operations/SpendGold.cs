using UnityEngine;
using Core;

namespace Gold
{
	[CreateAssetMenu(fileName = "Spend Gold", menuName = "Gold/Operations/Spend")]
	public sealed class SpendGold : ConsumeOperation, IOperationWithParameter
	{
		[SerializeField] private Gold _gold;
		[Min(1)] [SerializeField] private int _defaultAmount = 1;

		public override PlayerDataValueInfo Info => _gold.Info;
		
		public IOperationParameter CreateDefaultParam() => new IntAmountParameter { Amount = _defaultAmount };
		
		public bool IsSupports(IOperationParameter parameter) => parameter is IntAmountParameter;
		
		public bool IsCanApply(IPlayerDataInfo data, IOperationParameter parameter)
		{
			var intParam = parameter as IntAmountParameter;
			return _gold.GetCurrent(data) >= Mathf.Max(1, intParam?.Amount ?? _defaultAmount);
		}
		public override bool IsCanApply(IPlayerDataInfo data) => _gold.GetCurrent(data) >= _defaultAmount;

		public void Apply(PlayerData data, IOperationParameter parameter)
		{
			var intParam = parameter as IntAmountParameter;
			_gold.Decrease(data, Mathf.Max(1, intParam?.Amount ?? _defaultAmount));
		}
		
		public override void Apply(PlayerData data) => _gold.Decrease(data, _defaultAmount);

#if UNITY_EDITOR
		private void OnValidate() => _defaultAmount = Mathf.Max(1, _defaultAmount);
#endif
	}
}