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
        public async Task CanCreateFranchiseAndQueryById()
        {
            var addFranchiseModel = new AddFranchiseModel { Name = "Test" };
            var createdAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.Franchises.ToString(), addFranchiseModel);
            var franchise = await fixture.HttpClient.GetJsonAsync<Franchise>($"{ Routes.Franchises }/{ createdAt.Id }");
            franchise.Name.Should().Be("Test");
        }

        [Fact]
        public async Task CanGetFranchiseList()
        {
            var addFranchiseModel1 = new AddFranchiseModel { Name = "Test1" };
            var addFranchiseModel2 = new AddFranchiseModel { Name = "Test2" };
            await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.Franchises.ToString(), addFranchiseModel1);
            await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.Franchises.ToString(), addFranchiseModel2);
            var franchises = await fixture.HttpClient.GetJsonAsync<PageResponse<Franchise>>($"{ Routes.Franchises }");
            franchises.Results.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CanDeleteFranchise()
        {
            var addFranchiseModel = new AddFranchiseModel { Name = "Test3" };
            var createdAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.Franchises.ToString(), addFranchiseModel);
            await fixture.HttpClient.DeleteAsync($"{ Routes.Franchises }/{ createdAt.Id }");
            await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.Franchises.ToString(), addFranchiseModel);
            var response = await fixture.HttpClient.GetAsync($"{ Routes.Franchises }/{ createdAt.Id }");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
