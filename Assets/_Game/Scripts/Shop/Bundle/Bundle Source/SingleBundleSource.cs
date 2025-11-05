using System.Collections.Generic;

namespace Shop
{
	public sealed class SingleBundleSource : IBundleSource
	{
		private readonly BundleData _selected;

		public SingleBundleSource(BundleData selected)
		{
			_selected = selected; 
		}
		
		public IReadOnlyList<BundleData> GetBundles()
			=> _selected != null ? new[] { _selected } : System.Array.Empty<BundleData>();
	}
}