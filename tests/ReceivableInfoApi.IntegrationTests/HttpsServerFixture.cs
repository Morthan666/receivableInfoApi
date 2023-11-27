using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReceivableInfoApi.WebApi;

namespace ReceivableInfoApi.IntegrationTests;

public class HttpServerFixture : WebApplicationFactory<Program>
{
    private bool _disposed;
    private IHost? _host;

    public HttpServerFixture()
    {
        ClientOptions.AllowAutoRedirect = false;
        ClientOptions.BaseAddress = new Uri("https://localhost");
    }

    public string ServerAddress
    {
        get
        {
            EnsureServer();
            return ClientOptions.BaseAddress.ToString();
        }
    }

    public override IServiceProvider Services
    {
        get
        {
            EnsureServer();
            return _host!.Services!;
        }
    }

    public HttpClient CreateHttpClient() => CreateDefaultClient();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = string.Empty;

        var config = new[]
        {
            KeyValuePair.Create<string, string?>("ConnectionString", connectionString)
        };

        var configRoot = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();

        builder.ConfigureAppConfiguration(configBuilder => configBuilder.AddInMemoryCollection(config));

        builder.UseUrls("http://127.0.0.1:0");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var testHost = builder.Build();

        builder.ConfigureWebHost(p => p.UseKestrel());

        _host = builder.Build();
        _host.Start();

        var server = _host.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>();

        ClientOptions.BaseAddress = addresses!.Addresses
            .Select(p => new Uri(p))
            .Last();

        testHost.Start();
        return testHost;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!_disposed)
        {
            if (disposing) _host?.Dispose();
            _disposed = true;
        }
    }

    private void EnsureServer()
    {
        if (_host is null)
            using (CreateDefaultClient())
            {
            }
    }
}