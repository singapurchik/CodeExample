using System;

namespace FAS
{
	public interface IZonesHolderInfo
	{
		public IZone CurrentZone { get;}
		
		public ZoneType CurrentZoneType { get; }

		public event Action<ZoneType> OnZoneChanged;
	}
}