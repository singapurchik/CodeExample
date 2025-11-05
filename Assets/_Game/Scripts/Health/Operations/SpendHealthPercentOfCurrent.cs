using UnityEngine;
using Core;

namespace Health
{
    [CreateAssetMenu(fileName = "Spend Health Percent of Current",
	    menuName = "Health/Operations/Spend (Percent of Current)")]
    public sealed class SpendHealthPercentOfCurrent : ConsumeOperation, IOperationWithParameter
    {
        [SerializeField] private Health _health;
        [Range(1, 100)] [SerializeField] private int _defaultPercent = 10;

        public override PlayerDataValueInfo Info => _health.Info;

        public IOperationParameter CreateDefaultParam() => new PercentAmountParameter { Percent = _defaultPercent };

        public bool IsSupports(IOperationParameter parameter) => parameter is PercentAmountParameter;

        public override bool IsCanApply(IPlayerDataInfo data)
            => IsCanApply(data, new PercentAmountParameter { Percent = _defaultPercent });

        public bool IsCanApply(IPlayerDataInfo data, IOperationParameter parameter)
        {
            var intParam = parameter as PercentAmountParameter;
            var percent = Mathf.Clamp(intParam?.Percent ?? _defaultPercent, 1, 100);

            var current = Mathf.Max(0, _health.GetCurrent(data));
            var need = Mathf.Max(1, (current * percent + 99) / 100);

            return current >= need;
        }

        public override void Apply(PlayerData data)
            => Apply(data, new PercentAmountParameter { Percent = _defaultPercent });

        public void Apply(PlayerData data, IOperationParameter parameter)
        {
            var intParam = parameter as PercentAmountParameter;
            var percent = Mathf.Clamp(intParam?.Percent ?? _defaultPercent, 1, 100);

            var current = Mathf.Max(0, _health.GetCurrent(data));
            var delta = Mathf.Max(1, (current * percent + 99) / 100);

            _health.Decrease(data, delta);
        }

#if UNITY_EDITOR
        private void OnValidate() => _defaultPercent = Mathf.Clamp(_defaultPercent, 1, 100);
#endif
    }
}
