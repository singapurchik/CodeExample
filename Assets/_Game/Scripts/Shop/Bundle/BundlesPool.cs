using Core;

namespace Shop
{
	public class BundlesPool : ObjectPool<Bundle>
	{
		protected override void InitializeObject(Bundle bundle)
		{
			bundle.OnBundleOutOfStock += ReturnToPool;
		}

		protected override void CleanupObject(Bundle bundle)
		{
			bundle.OnBundleOutOfStock -= ReturnToPool;
		}
	}
}