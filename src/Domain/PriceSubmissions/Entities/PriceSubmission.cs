using GasWeb.Shared;
using GasWeb.Shared.PriceSubmissions;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public int TotalScore => Ratings.Sum(x => x.Value switch
            {
                PriceSubmissionRatingValue.Negative => -1,
                PriceSubmissionRatingValue.Neutral => 0,
                PriceSubmissionRatingValue.Positive => 1
            });
    }
}
