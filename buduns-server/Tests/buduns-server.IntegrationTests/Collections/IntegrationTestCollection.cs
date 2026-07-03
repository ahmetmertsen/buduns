using buduns_server.IntegrationTests.Fixtures;

namespace buduns_server.IntegrationTests.Collections;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class IntegrationTestCollection : ICollectionFixture<BudunsWebApplicationFactory>
{
    public const string Name = "Buduns integration tests";
}
