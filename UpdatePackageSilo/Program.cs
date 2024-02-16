using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "DeviceUpdatesCluster";
        options.ServiceId = "UpdatePackageSilo";
    });

    siloBuilder.AddLogStorageBasedLogConsistencyProvider("LogStorage");

    siloBuilder.AddAdoNetGrainStorage("AzureSqlStorage", options =>
    {
        options.Invariant = "System.Data.SqlClient"; //https://learn.microsoft.com/en-us/dotnet/orleans/host/configuration-guide/adonet-configuration#persistence
        options.ConnectionString = "<SQLSERVERCONNECTIONSTRING>";
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
