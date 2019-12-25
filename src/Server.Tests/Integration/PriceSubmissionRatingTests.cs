using FluentAssertions;
using GasWeb.Shared;
using GasWeb.Shared.GasStations;
using GasWeb.Shared.PriceSubmissions;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Xunit;

namespace GasWeb.Server.Tests.Integration
{
    [Collection(IntegrationTestCollection.Name)]
    public class PriceSubmissionRatingTests
    {
        private readonly IntegrationTestFixture fixture;

        public PriceSubmissionRatingTests(IntegrationTestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task NoRatings_Add_RatingIsAddedSuccessfully()
        {
            // given

            var submissionId = await CreatePriceSumission();

            // when

            var ratingModel = new RatePriceSubmissionModel { Value = PriceSubmissionRatingValue.Positive };
            await fixture.HttpClient.PutJsonAsync($"{ Routes.PriceSubmissions }/{ submissionId}/rate", ratingModel);

            // then

            var submission = await fixture.HttpClient.GetJsonAsync<PriceSubmission>($"{ Routes.PriceSubmissions }/{ submissionId }");
            submission.Ratings.Should().ContainSingle().Which.Value.Should().Be(PriceSubmissionRatingValue.Positive);
        }

        [Fact]
        public async Task PositiveSubmissionRating_AddNegativeRate_UserRatingIsChangedToNegative()
        {
            // given

            var submissionId = await CreatePriceSumission();
            var positiveRatingModel = new RatePriceSubmissionModel { Value = PriceSubmissionRatingValue.Positive };
            await fixture.HttpClient.PutJsonAsync($"{ Routes.PriceSubmissions }/{ submissionId}/rate", positiveRatingModel);

            // when

            var negativeRatingModel = new RatePriceSubmissionModel { Value = PriceSubmissionRatingValue.Negative };
            await fixture.HttpClient.PutJsonAsync($"{ Routes.PriceSubmissions }/{ submissionId}/rate", negativeRatingModel);

            // then

            var submission = await fixture.HttpClient.GetJsonAsync<PriceSubmission>($"{ Routes.PriceSubmissions }/{ submissionId }");
            submission.Ratings.Should().ContainSingle().Which.Value.Should().Be(PriceSubmissionRatingValue.Negative);
        }

        [Fact]
        public async Task PositiveSubmissionRating_AddPositiveRate_UserRatingIsUnchanged()
        {
            // given

            var submissionId = await CreatePriceSumission();
            var positiveRatingModel = new RatePriceSubmissionModel { Value = PriceSubmissionRatingValue.Positive };
            await fixture.HttpClient.PutJsonAsync($"{ Routes.PriceSubmissions }/{ submissionId}/rate", positiveRatingModel);

            // when

            await fixture.HttpClient.PutJsonAsync($"{ Routes.PriceSubmissions }/{ submissionId}/rate", positiveRatingModel);

            // then

            var submission = await fixture.HttpClient.GetJsonAsync<PriceSubmission>($"{ Routes.PriceSubmissions }/{ submissionId }");
            submission.Ratings.Should().ContainSingle().Which.Value.Should().Be(PriceSubmissionRatingValue.Positive);
        }

        private async Task<long> CreatePriceSumission()
        {
            var addModel = new AddGasStationModel { Name = "Amic Energy" };
            var gasStationCreatedAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.GasStations.ToString(), addModel);
            var submitModel = new SubmitPriceModel
            {
                Amount = 5,
                FuelType = FuelType.Diesel,
                GasStationId = gasStationCreatedAt.Id
            };

            var priceSubmitedAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.PriceSubmissions.ToString(), submitModel);
            return priceSubmitedAt.Id;
        }
    }
}
