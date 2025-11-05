using UnityEngine;
using Core;

namespace Health
{
	[CreateAssetMenu(fileName = "Spend Health", menuName = "Health/Operations/Spend")]
	public sealed class SpendHealth : ConsumeOperation, IOperationWithParameter
	{
		[SerializeField] private Health _health;
		[Min(1)] [SerializeField] private int _defaultAmount = 1;

		public override PlayerDataValueInfo Info => _health.Info;
		
		public IOperationParameter CreateDefaultParam() => new IntAmountParameter { Amount = _defaultAmount };
		
		public bool IsSupports(IOperationParameter parameter) => parameter is IntAmountParameter;
		
		public bool IsCanApply(IPlayerDataInfo data, IOperationParameter parameter)
		{
			var intParam = parameter as IntAmountParameter;
			return _health.GetCurrent(data) >= Mathf.Max(1, intParam?.Amount ?? _defaultAmount);
		}
		public override bool IsCanApply(IPlayerDataInfo data) => _health.GetCurrent(data) >= _defaultAmount;

		public void Apply(PlayerData data, IOperationParameter parameter)
		{
			var intParam = parameter as IntAmountParameter;
			_health.Decrease(data, Mathf.Max(1, intParam?.Amount ?? _defaultAmount));
		}
		
		public override void Apply(PlayerData data) => _health.Decrease(data, _defaultAmount);

#if UNITY_EDITOR
		private void OnValidate() => _defaultAmount = Mathf.Max(1, _defaultAmount);
#endif
	}
}