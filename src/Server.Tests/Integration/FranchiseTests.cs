using FluentAssertions;
using GasWeb.Shared;
using GasWeb.Shared.Franchises;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace GasWeb.Server.Tests.Integration
{
    [Collection(IntegrationTestCollection.Name)]
    public class FranchiseTests
    {
        private readonly IntegrationTestFixture fixture;

        public FranchiseTests(IntegrationTestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task NoFranchises_Create_FranchiseIsCreatedSuccessfully()
        {
            // given

            // no franchises

            // when

            var addFranchiseModel = new AddFranchiseModel { Name = "Test" };
            var createdAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.Franchises.ToString(), addFranchiseModel);

            // then

            var franchise = await fixture.HttpClient.GetJsonAsync<Franchise>($"{ Routes.Franchises }/{ createdAt.Id }");
            franchise.Name.Should().Be("Test");
        }

        [Fact]
        public async Task SomeFranchises_GetList_FranchisesAreRetrieved()
        {
            // given

            var addFranchiseModel1 = new AddFranchiseModel { Name = "Test1" };
            var addFranchiseModel2 = new AddFranchiseModel { Name = "Test2" };
            await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.Franchises.ToString(), addFranchiseModel1);
            await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.Franchises.ToString(), addFranchiseModel2);

            // when

            var franchises = await fixture.HttpClient.GetJsonAsync<PageResponse<Franchise>>($"{ Routes.Franchises }");

            // then

            franchises.Results.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Franchise_Delete_FranchiseIsDeleted()
        {
            // given

            var addFranchiseModel = new AddFranchiseModel { Name = "Test3" };
            var createdAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.Franchises.ToString(), addFranchiseModel);

            // when

            await fixture.HttpClient.DeleteAsync($"{ Routes.Franchises }/{ createdAt.Id }");

            // then

            var response = await fixture.HttpClient.GetAsync($"{ Routes.Franchises }/{ createdAt.Id }");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Franchise_UpdateName_FranchiseNameIsUpdated()
        {
            // given

            var addFranchiseModel = new AddFranchiseModel { Name = "Test4" };
            var createdAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.Franchises.ToString(), addFranchiseModel);

            // when

            var updateFranchiseModel = new UpdateFranchiseModel { Name = "Test5" };
            await fixture.HttpClient.PutJsonAsync($"{ Routes.Franchises }/{ createdAt.Id }", updateFranchiseModel);

            // then

            var franchise = await fixture.HttpClient.GetJsonAsync<Franchise>($"{ Routes.Franchises }/{ createdAt.Id }");
            franchise.Name.Should().Be("Test5");
        }
    }
}
