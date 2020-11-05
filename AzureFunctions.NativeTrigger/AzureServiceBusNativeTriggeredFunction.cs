using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace AzureFunctions.NativeTrigger
{
    public class AzureServiceBusNativeTriggeredFunction
    {
        private const string EndpointName = "ASBTriggerQueue";

        private static FunctionEndpoint endpoint = new FunctionEndpoint(c =>
        {
            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json")
                .AddEnvironmentVariables()
                .Build();

            var config = new ServiceBusTriggeredEndpointConfiguration(EndpointName);
            var dbContextBuilder = new DbContextOptionsBuilder<MyDbContext>();
            dbContextBuilder.UseSqlServer(configurationRoot.GetConnectionString("MyDbConnectionString"));

            // should be in a UOW to dispose the context and so on
            config.AdvancedConfiguration.RegisterComponents(r => r.ConfigureComponent(() => new MyDbContext(dbContextBuilder.Options), DependencyLifecycle.InstancePerCall));

            return config;
        });

        [FunctionName("Function1")]
        public static Task Run([ServiceBusTrigger(EndpointName)] Message message, ExecutionContext context, ILogger log)
        {
            return endpoint.Process(message, context, log);
        }
    }
}