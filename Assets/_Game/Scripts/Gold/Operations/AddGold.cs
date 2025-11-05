using UnityEngine;
using Core;

namespace Gold
{
	[CreateAssetMenu(fileName = "Add Gold", menuName = "Gold/Operations/Add")]
	public sealed class AddGold : ProvideOperation, IOperationWithParameter
	{
		[SerializeField] private Gold _gold;
		[Min(1)] [SerializeField] private int _defaultAmount = 1;

		public override PlayerDataValueInfo Info => _gold.Info;
		
		public IOperationParameter CreateDefaultParam() => new IntAmountParameter { Amount = _defaultAmount };
		
		public bool IsSupports(IOperationParameter parameter) => parameter is IntAmountParameter;
		
		public bool IsCanApply(IPlayerDataInfo data, IOperationParameter parameter) => IsCanApply(data);

		public void Apply(PlayerData data, IOperationParameter parameter)
		{
			var intParam = parameter as IntAmountParameter;
			_gold.Increase(data, Mathf.Max(1, intParam?.Amount ?? _defaultAmount));
		}

		public override void Apply(PlayerData data) => _gold.Increase(data, _defaultAmount);

#if UNITY_EDITOR
		private void OnValidate() => _defaultAmount = Mathf.Max(1, _defaultAmount);
#endif
	}
}