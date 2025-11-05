using Core;

namespace Shop
{
	public class ShopScenesLoader : SceneLoaderBase, IShopScenesLoader
	{
		private readonly SceneLoadingData _bundleInfo;
		private readonly SceneLoadingData _shop;

		public ShopScenesLoader(SceneLoadingData bundleInfo, SceneLoadingData shop)
		{
			_bundleInfo = bundleInfo;
			_shop = shop;
		}
		
		public void LoadBundleInfoScene() => LoadScene(_bundleInfo);
		
		public void LoadShopScene() => LoadScene(_shop);
	}
}