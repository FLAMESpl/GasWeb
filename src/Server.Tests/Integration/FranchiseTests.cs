using FluentAssertions;
using GasWeb.Shared.Franchises;
using Microsoft.AspNetCore.Components;
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
            var franchise = await fixture.HttpClient.GetJsonAsync<Franchise>(Routes.Franchises + createdAt.Id.ToString());
            franchise.Name.Should().Be("Test");
        }
    }
}
