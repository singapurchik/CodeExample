using UnityEngine;

namespace FAS
{
	[CreateAssetMenu(fileName = "Player Health Behaviour Data", menuName = "FAS/Player Health Behaviour Data", order = 0)]
	public class PlayerHealthBehaviourData : ScriptableObject
	{
		[field: SerializeField] public PlayerHealthBehaviourType Type { get; private set; }
		[field: SerializeField] public Color PulseColor { get; private set; }
		[field: SerializeField] public float PulseSpeed { get; private set; }
		[field: SerializeField] public float AnimatorBehaviourTypeIndex { get; private set; }
		[field: SerializeField] public int HealthThreshold { get; private set; }
		
		[Range(0, 1)] [SerializeField] public float _injectorPenDropBonusNormalized;

		public float InjectorPenDropBonusNormalized => _injectorPenDropBonusNormalized;
	}
}