using System;

namespace GasWeb.Domain.PriceSubmissions.Entities
{
    internal class PriceSubmissionRating
    {
        public PriceSubmissionRating(long priceSubmissionId, long userId, bool positive, DateTime submitedAt)
        {
            PriceSubmissionId = priceSubmissionId;
            UserId = userId;
            Positive = positive;
            SubmitedAt = submitedAt;
        }

        public long PriceSubmissionId { get; private set; }
        public long UserId { get; private set; }
        public bool Positive { get; private set; }
        public DateTime SubmitedAt { get; private set; }
    }
}
