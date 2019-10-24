using System;

namespace GasWeb.Shared.GasStations
{
    public class PriceSubmission
    {
        public PriceSubmission(long id, long createdByUserId, DateTime submissionDate,
            long gasStationId, decimal amount, FuelType fuelType)
        {
            Id = id;
            CreatedByUserId = createdByUserId;
            SubmissionDate = submissionDate;
            GasStationId = gasStationId;
            Amount = amount;
            FuelType = fuelType;
        }

        public long Id { get; }
        public long CreatedByUserId { get; }
        public DateTime SubmissionDate { get; }
        public long GasStationId { get; }
        public decimal Amount { get; }
        public FuelType FuelType { get; }
    }
}
