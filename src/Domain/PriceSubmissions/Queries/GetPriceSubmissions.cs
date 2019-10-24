using GasWeb.Shared;

namespace GasWeb.Domain.PriceSubmissions.Queries
{
    public class GetPriceSubmissions
    {
        public GetPriceSubmissions(long? gasStationId, FuelType? fuelType, long? createdByUserId, int pageNumber, int pageSize)
        {
            GasStationId = gasStationId;
            FuelType = fuelType;
            CreatedByUserId = createdByUserId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public long? GasStationId { get; }
        public FuelType? FuelType { get; }
        public long? CreatedByUserId { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
    }
}
