namespace FAS
{
	public interface IZonesHolder : IZonesHolderInfo
	{
		public IZone GetZoneByType(ZoneType zoneType);
	}
}