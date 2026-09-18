using Microsoft.AspNetCore.Mvc.Testing;

namespace VetCommission.IntegrationTests.WebApi;

[CollectionDefinition(Name)]
public sealed class WebApiCollection : ICollectionFixture<WebApplicationFactory<Program>>
{
    public const string Name = "WebApi";
}
