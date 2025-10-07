namespace FAS.Apartments
{
	public interface IReadOnlyApartment
	{
		public int CurrentApartmentNumber { get; }
		public int ApartmentsCount { get; }
	}
}