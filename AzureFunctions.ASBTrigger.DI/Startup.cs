using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

[assembly: FunctionsStartup(typeof(Startup))]

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var services = builder.Services;

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