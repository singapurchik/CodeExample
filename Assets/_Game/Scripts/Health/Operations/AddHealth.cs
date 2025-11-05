using UnityEngine;
using Core;

namespace Health
{
	[CreateAssetMenu(fileName = "Add Health", menuName = "Health/Operations/Add")]
	public sealed class AddHealth : ProvideOperation, IOperationWithParameter
	{
		[SerializeField] private Health _health;
		[Min(1)] [SerializeField] private int _defaultAmount = 1;

		public override PlayerDataValueInfo Info => _health.Info;

		public IOperationParameter CreateDefaultParam() => new IntAmountParameter { Amount = _defaultAmount };
		
		public bool IsSupports(IOperationParameter parameter) => parameter is IntAmountParameter;
		
		public bool IsCanApply(IPlayerDataInfo data, IOperationParameter parameter) => true;
		
		public void Apply(PlayerData data, IOperationParameter parameter)
		{
			var intParam = parameter as IntAmountParameter;
			_health.Increase(data, Mathf.Max(1, intParam?.Amount ?? _defaultAmount));
		}

		public override void Apply(PlayerData data) => _health.Increase(data, _defaultAmount);

#if UNITY_EDITOR
		private void OnValidate() => _defaultAmount = Mathf.Max(1, _defaultAmount);
#endif
	}
}