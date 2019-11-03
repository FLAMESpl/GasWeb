using GasWeb.Shared.PriceSubmissions;
using System;

namespace GasWeb.Domain.PriceSubmissions.Entities
{
    internal class PriceSubmissionRating
    {
        public PriceSubmissionRating(long priceSubmissionId, long userId, PriceSubmissionRatingValue value, DateTime submitedAt)
        {
            PriceSubmissionId = priceSubmissionId;
            UserId = userId;
            Value = value;
            SubmitedAt = submitedAt;
        }

        public long PriceSubmissionId { get; private set; }
        public long UserId { get; private set; }
        public PriceSubmissionRatingValue Value { get; private set; }
        public DateTime SubmitedAt { get; private set; }
    }
}
