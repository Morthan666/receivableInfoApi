namespace ReceivableInfoApi.IntegrationTests;

[CollectionDefinition(Name)]
public sealed class HttpServerCollection : ICollectionFixture<HttpServerFixture>
{
    public const string Name = "ReceivableInfoApi HTTP server collection";
}