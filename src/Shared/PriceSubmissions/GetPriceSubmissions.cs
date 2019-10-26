namespace GasWeb.Shared.PriceSubmissions
{
    public class GetPriceSubmissions
    {
        public GetPriceSubmissions()
        {
        }

        public GetPriceSubmissions(long? gasStationId, FuelType fuelTypes, long? createdByUserId, int pageNumber, int pageSize)
        {
            GasStationId = gasStationId;
            FuelTypes = fuelTypes;
            CreatedByUserId = createdByUserId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public long? GasStationId { get; set; }
        public FuelType FuelTypes { get; set; }
        public long? CreatedByUserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
