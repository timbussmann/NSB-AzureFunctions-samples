using System;
using AzureFunctions.ASBTrigger.DI;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

[assembly: FunctionsStartup(typeof(Startup))]

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var services = builder.Services;

        var configurationRoot = builder.GetContext().Configuration;

        services.AddSingleton<IConfiguration>(configurationRoot);

        services.AddDbContext<MyDbContext>(options =>
        {
            var connectionString = configurationRoot.GetConnectionString("MyDbConnectionString");
            options.UseSqlServer(connectionString);
            options.EnableServiceProviderCaching(false);
        });

        builder.UseNServiceBus(() =>
        {
            //TODO FromAttributes does not work as the function declaration is not in the stack when this method is called.
            //var configuration = ServiceBusTriggeredEndpointConfiguration.FromAttributes();

            var configuration = new ServiceBusTriggeredEndpointConfiguration(AzureServiceBusTriggerFunction.EndpointName, "AzureWebJobsServiceBus");
            return configuration;
        });
    }
}