using Xunit;

namespace GasWeb.Server.Tests.Integration
{
    [CollectionDefinition(Name)]
    public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
    {
        public const string Name = "GasWeb Application Fixture";
    }
}
