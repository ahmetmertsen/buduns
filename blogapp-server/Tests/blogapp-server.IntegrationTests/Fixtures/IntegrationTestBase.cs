using blogapp_server.IntegrationTests.Collections;

namespace blogapp_server.IntegrationTests.Fixtures;

[Collection(IntegrationTestCollection.Name)]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected IntegrationTestBase(BlogAppWebApplicationFactory factory)
    {
        Factory = factory;
    }

    protected BlogAppWebApplicationFactory Factory { get; }

    public Task InitializeAsync() => Factory.ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;
}
