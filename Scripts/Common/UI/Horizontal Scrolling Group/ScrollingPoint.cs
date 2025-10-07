using UnityEngine;

namespace FAS.UI
{
	public class ScrollingPoint : MonoBehaviour
	{
		public enum PriorityType
		{
			High,
			Middle,
			Low
		}
		
		[field: SerializeField] public PriorityType Priority { get; set; }
	}
}