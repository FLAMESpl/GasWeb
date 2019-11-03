using GasWeb.Shared;
using System;
using System.Collections.Generic;

namespace GasWeb.Domain.PriceSubmissions.Entities
{
    internal class PriceSubmission : AuditEntity
    {
        public PriceSubmission(long gasStationId, DateTime submissionDate, decimal amount, FuelType fuelType)
        {
            GasStationId = gasStationId;
            SubmissionDate = submissionDate;
            Amount = amount;
            FuelType = fuelType;
        }

        public PriceSubmission(long id, long createdByUserId, long modifiedByUserId, DateTime lastModified,
            long gasStationId, DateTime submissionDate, decimal amount, FuelType fuelType)
                : base(id, createdByUserId, modifiedByUserId, lastModified)
        {
            GasStationId = gasStationId;
            SubmissionDate = submissionDate;
            Amount = amount;
            FuelType = fuelType;
        }

        public long GasStationId { get; private set; }
        public DateTime SubmissionDate { get; private set; }
        public decimal Amount { get; private set; }
        public FuelType FuelType { get; private set; }

        public ICollection<PriceSubmissionRating> Ratings { get; private set; }
    }
}
