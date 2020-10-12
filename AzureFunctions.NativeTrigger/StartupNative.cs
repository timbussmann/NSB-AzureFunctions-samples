using System;
using AzureFunctions.NativeTrigger;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(StartupNative))]

public class StartupNative : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var services = builder.Services;

        var configurationRoot = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("local.settings.json")
            //.AddUserSecrets<StartupNative>()
            .AddEnvironmentVariables()
            .Build();
        services.AddSingleton<IConfiguration>(configurationRoot);

        services.AddDbContext<MyDbContext>(delegate(DbContextOptionsBuilder options)
        {
            var connectionString = configurationRoot.GetConnectionString("MyDbConnectionString");
            options.UseSqlServer(connectionString);
        });
    }
}