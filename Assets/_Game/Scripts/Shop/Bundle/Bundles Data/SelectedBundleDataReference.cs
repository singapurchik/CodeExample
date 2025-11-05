using UnityEngine;

namespace Shop
{
	[CreateAssetMenu(fileName = "Selected Bundle Data Reference", menuName = "Shop/Selected Bundle Data Referencer")]
	public class SelectedBundleDataReference : ScriptableObject
	{
		[HideInInspector] [SerializeField] private BundleData _data;

		public BundleData Data => _data;

		public void Set(BundleData bundle) => _data = bundle;
		
		public void Clear() => _data = null;
	}
}