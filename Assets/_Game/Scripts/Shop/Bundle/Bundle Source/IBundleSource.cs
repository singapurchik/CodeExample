using System.Collections.Generic;

namespace Shop
{
	public interface IBundleSource
	{
		public IReadOnlyList<BundleData> GetBundles();
	}
}