using FluentAssertions;
using GasWeb.Shared;
using GasWeb.Shared.GasStations;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace GasWeb.Server.Tests.Integration
{
    [Collection(IntegrationTestCollection.Name)]
    public class GasStationTests
    {
        private readonly IntegrationTestFixture fixture;

        public GasStationTests(IntegrationTestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task NoGasStations_Create_GasStationIsCreatedSuccessfully()
        {
            // given

            // no gas stations

            // when

            var addGasStationModel = new AddGasStationModel { Name = "Amic Energy" };
            var createdAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.GasStations.ToString(), addGasStationModel);

            // then

            var gasStation = await fixture.HttpClient.GetJsonAsync<GasStation>($"{ Routes.GasStations }/{ createdAt.Id }");
            gasStation.Name.Should().Be("Amic Energy");
        }

        [Fact]
        public async Task SomeGasStations_GetList_SomeGasStationsAreRetrieved()
        {
            // given

            var addGasStationModel = new AddGasStationModel { Name = "Amic Energy" };
            await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.GasStations.ToString(), addGasStationModel);
            await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.GasStations.ToString(), addGasStationModel);

            // when

            var gasStationList = await fixture.HttpClient.GetJsonAsync<PageResponse<GasStation>>(Routes.GasStations.ToString());

            // then

            gasStationList.Results.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GasStation_Delete_GasStationIsDeletedSuccessfully()
        {
            // given

            var addGasStationModel = new AddGasStationModel { Name = "Amic Energy" };
            var createdAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.GasStations.ToString(), addGasStationModel);

            // when

            await fixture.HttpClient.DeleteAsync($"{ Routes.GasStations }/{ createdAt.Id }");

            // then

            var response = await fixture.HttpClient.GetAsync($"{ Routes.GasStations }/{ createdAt.Id }");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GasStation_UpdateName_GasStationNameIsUpdated()
        {
            // given

            var addGasStationModel = new AddGasStationModel { Name = "Łukoil" };
            var createdAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.GasStations.ToString(), addGasStationModel);

            // when

            var updateGasStationModel = new UpdateGasStationModel { Name = "Amic Energy" };
            await fixture.HttpClient.PutJsonAsync($"{ Routes.GasStations }/{ createdAt.Id }", updateGasStationModel);

            // then

            var gasStation = await fixture.HttpClient.GetJsonAsync<GasStation>($"{ Routes.GasStations }/{ createdAt.Id }");
            gasStation.Name.Should().Be("Amic Energy");
        }
    }
}
