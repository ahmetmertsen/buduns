using blogapp_server.IntegrationTests.Fixtures;

namespace blogapp_server.IntegrationTests.Collections;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class IntegrationTestCollection : ICollectionFixture<BlogAppWebApplicationFactory>
{
    public const string Name = "BlogApp integration tests";
}
