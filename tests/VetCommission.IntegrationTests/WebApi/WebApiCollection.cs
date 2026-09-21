namespace VetCommission.IntegrationTests.WebApi;

[CollectionDefinition(Name)]
public sealed class WebApiCollection : ICollectionFixture<WebApiTestFactory>
{
    public const string Name = "WebApi";
}
