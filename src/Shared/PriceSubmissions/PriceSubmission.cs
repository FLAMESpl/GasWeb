using System;

namespace GasWeb.Shared.PriceSubmissions
{
    public class PriceSubmission
    {
        public PriceSubmission() { }

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

        public long Id { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public long GasStationId { get; set; }
        public decimal Amount { get; set; }
        public FuelType FuelType { get; set; }
    }
}
