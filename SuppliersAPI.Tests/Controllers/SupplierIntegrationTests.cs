using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using SuppliersApi;

namespace SuppliersAPI.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
  private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
      .WithImage("mongo:6.0")
      .WithCleanUp(true)
      .WithPortBinding(27018, 27017)
      .Build();

  public string ConnectionString => _mongoDbContainer.GetConnectionString();

  public async Task InitializeAsync()
  {
    await _mongoDbContainer.StartAsync();
  }

  public async Task DisposeAsync()
  {
    await _mongoDbContainer.DisposeAsync();
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureAppConfiguration((context, config) =>
    {
      var settings = new Dictionary<string, string?>
      {
        ["MongoDb:ConnectionString"] = ConnectionString,
        ["MongoDb:DatabaseName"] = "test_suppliers"
      };

      config.AddInMemoryCollection(settings!);
    });

    builder.ConfigureServices(services =>
    {
      // opcional: limpiar servicios si querés hacer mock de algo
    });
  }
}
