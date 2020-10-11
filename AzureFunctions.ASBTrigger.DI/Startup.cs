using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

[assembly: FunctionsStartup(typeof(Startup))]

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var services = builder.Services;

        var configurationRoot = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("local.settings.json")
            .AddUserSecrets<Startup>()
            .AddEnvironmentVariables()
            .Build();
        services.AddSingleton<IConfiguration>(configurationRoot);

        services.AddScoped(typeof(MyService));
        services.AddScoped<IMyService>(sp => sp.GetRequiredService<MyService>());

        services.AddSingleton(sp => new FunctionEndpoint(executionContext =>
        {
            var configuration = ServiceBusTriggeredEndpointConfiguration.FromAttributes();

            configuration.UseSerialization<NewtonsoftSerializer>();

            configuration.LogDiagnostics();

            configuration.AdvancedConfiguration.UseContainer(new CustomServiceProviderFactory(services));

            return configuration;
        }));
    }
}

public class MyService : IMyService
{
    public string SayHello() => "Hello!";
}

public interface IMyService
{
    string SayHello();
}