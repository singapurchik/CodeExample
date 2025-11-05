using System.Collections.Generic;

namespace Shop
{
	public sealed class ListBundleSource : IBundleSource
	{
		private readonly IReadOnlyList<BundleData> _bundles;

		public ListBundleSource(IReadOnlyList<BundleData> bundles)
		{
			_bundles = bundles;
		}
		
		public IReadOnlyList<BundleData> GetBundles() => _bundles;
	}
}