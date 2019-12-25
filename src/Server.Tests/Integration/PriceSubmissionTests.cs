using FluentAssertions;
using GasWeb.Shared;
using GasWeb.Shared.GasStations;
using GasWeb.Shared.PriceSubmissions;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GasWeb.Server.Tests.Integration
{
    [Collection(IntegrationTestCollection.Name)]
    public class PriceSubmissionTests
    {
        private readonly IntegrationTestFixture fixture;

        public PriceSubmissionTests(IntegrationTestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GasStationWithNoSubmissions_SubmitGasPrice_PricesIsSubmittedSuccessfully()
        {
            // given

            var gasStationId = await CreateGasStation();

            // when

            var submitPriceModel = new SubmitPriceModel
            {
                Amount = 5,
                FuelType = FuelType.Petrol,
                GasStationId = gasStationId
            };

            var createdAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.PriceSubmissions.ToString(), submitPriceModel);

            // then

            var priceSubmission = await fixture.HttpClient.GetJsonAsync<PriceSubmission>($"{ Routes.PriceSubmissions }/{ createdAt.Id }");
            priceSubmission.Should().BeEquivalentTo(submitPriceModel);
        }

        [Fact]
        public async Task GasStationWithDieselPriceSubmitted_SubmitPetrolPrice_PricesIsSubmittedSuccessfully()
        {
            // given

            var gasStationId = await CreateGasStation();

            var submitPriceModelDiesel = new SubmitPriceModel
            {
                Amount = 4,
                FuelType = FuelType.Diesel,
                GasStationId = gasStationId
            };

            await fixture.HttpClient.PostJsonAsync(Routes.PriceSubmissions.ToString(), submitPriceModelDiesel);

            // when

            var submitPriceModelPetrol = new SubmitPriceModel
            {
                Amount = 5,
                FuelType = FuelType.Petrol,
                GasStationId = gasStationId
            };

            var createdAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.PriceSubmissions.ToString(), submitPriceModelPetrol);

            // then

            var priceSubmission = await fixture.HttpClient.GetJsonAsync<PriceSubmission>($"{ Routes.PriceSubmissions }/{ createdAt.Id }");
            priceSubmission.Should().BeEquivalentTo(submitPriceModelPetrol);
        }

        [Fact]
        public async Task GasStationWithDieselPriceSubmitted_SubmitAnotherDieselPrice_PricesSubmissionIsRejected()
        {
            // given

            var gasStationId = await CreateGasStation();

            var submitPriceModel1 = new SubmitPriceModel
            {
                Amount = 4,
                FuelType = FuelType.Diesel,
                GasStationId = gasStationId
            };

            await fixture.HttpClient.PostJsonAsync(Routes.PriceSubmissions.ToString(), submitPriceModel1);

            // when

            var submitPriceModel2 = new SubmitPriceModel
            {
                Amount = 5,
                FuelType = FuelType.Diesel,
                GasStationId = gasStationId
            };

            var jsonContent = JsonConvert.SerializeObject(submitPriceModel2);
            var request = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await fixture.HttpClient.PostAsync(Routes.PriceSubmissions, request);

            // then

            response.IsSuccessStatusCode.Should().BeFalse();
        }

        [Fact]
        public async Task GasStationWithSomePriceSubmissions_GetList_SomePriceSubmissionsAreRetrived()
        {
            // given

            var gasStationId = await CreateGasStation();

            var submitPriceModel1 = new SubmitPriceModel
            {
                Amount = 5,
                FuelType = FuelType.Petrol,
                GasStationId = gasStationId
            };

            var submitPriceModel2 = new SubmitPriceModel
            {
                Amount = 4,
                FuelType = FuelType.Diesel,
                GasStationId = gasStationId
            };

            await fixture.HttpClient.PostJsonAsync(Routes.PriceSubmissions.ToString(), submitPriceModel1);
            await fixture.HttpClient.PostJsonAsync(Routes.PriceSubmissions.ToString(), submitPriceModel2);

            // when

            var priceSubmissions = await fixture.HttpClient.GetJsonAsync<PageResponse<PriceSubmission>>(Routes.PriceSubmissions.ToString());
            priceSubmissions.Results.Should().NotBeEmpty();
        }

        [Fact]
        public async Task PriceSubmission_Delete_PriceSubmissionIsDeleted()
        {
            // given

            var gasStationId = await CreateGasStation();

            var submitPriceModel = new SubmitPriceModel
            {
                Amount = 5,
                FuelType = FuelType.Petrol,
                GasStationId = gasStationId
            };

            var createdAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.PriceSubmissions.ToString(), submitPriceModel);

            // when

            await fixture.HttpClient.DeleteAsync($"{ Routes.PriceSubmissions }/{ createdAt.Id }");

            // then

            var response = await fixture.HttpClient.GetAsync($"{ Routes.PriceSubmissions }/{ createdAt.Id }");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task<long> CreateGasStation()
        {
            var addModel = new AddGasStationModel { Name = "Amic Energy" };
            var createdAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.GasStations.ToString(), addModel);
            return createdAt.Id;
        }
    }
}
