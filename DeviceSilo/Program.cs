using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using DeviceSilo;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(AppContext.BaseDirectory))
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables();

var configurationRoot = configurationBuilder.Build();

var test = configurationRoot.GetSection("SqlServerConnectionString");

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "DeviceUpdatesCluster";
        options.ServiceId = "DeviceSilo";
    });

    siloBuilder.AddLogStorageBasedLogConsistencyProvider("LogStorage");

    siloBuilder.AddAdoNetGrainStorage("AzureSqlStorage", options =>
    {
        options.Invariant = "System.Data.SqlClient"; //https://learn.microsoft.com/en-us/dotnet/orleans/host/configuration-guide/adonet-configuration#persistence
        options.ConnectionString = " <SQLSERVERCONNECTIONSTRING>";
    });

    siloBuilder.AddAzureTableGrainStorage("PubSubStore", options =>
    {
        options.ConfigureTableServiceClient("AzureTableStorageConnectionString");
    });

    siloBuilder.AddEventHubStreams(
            "my-stream-provider",
            (ISiloEventHubStreamConfigurator configurator) =>
            {
                configurator.ConfigureEventHub(builder => builder.Configure(options =>
                    options.ConfigureEventHubConnection("",
                    "decog-eventhub-eastus-kew",
                    "$Default")));

                // We plug here our custom DataAdapter for Event Hub
                configurator.UseDataAdapter((sp, n) => ActivatorUtilities.CreateInstance<CustomDataAdapter>(sp));

                configurator.UseAzureTableCheckpointer(
                    builder => builder.Configure(options =>
                    {
                        options.ConfigureTableServiceClient("AzureTableStorageConnectionString");
                        options.PersistInterval = TimeSpan.FromSeconds(10);
                    }));
            });

    siloBuilder.UseDashboard(x =>
    {
        x.HostSelf = true;
        x.Port = 8080;
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
