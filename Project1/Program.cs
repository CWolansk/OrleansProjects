using Orleans.Runtime;
using Orleans.Storage;
using Microsoft.Extensions.Hosting;
using Orleans.Hosting;
using Microsoft.VisualBasic;
using Project1;
using Microsoft.Extensions.Options;
using Orleans.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();

    siloBuilder.Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "DeviceUpdatesCluster";
        options.ServiceId = "DeviceSilo";
    });

    siloBuilder.AddLogStorageBasedLogConsistencyProvider("LogStorage");

    siloBuilder.AddAdoNetGrainStorage("AzureSqlStorage", options =>
    {
        options.Invariant = "System.Data.SqlClient"; //https://learn.microsoft.com/en-us/dotnet/orleans/host/configuration-guide/adonet-configuration#persistence
        options.ConnectionString = "<SQLSERVERCONNECTIONSTRING>";
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
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
