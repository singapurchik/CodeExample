using UnityEngine;

namespace FAS.Items
{
	public abstract class UsableItemEffect : MonoBehaviour
	{
		public abstract ItemType Type { get; }
		
		public abstract void Play();
	}
}