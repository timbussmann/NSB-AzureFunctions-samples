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

        private static FunctionEndpoint endpoint = new FunctionEndpoint(functionExecutionContext =>
        {
            var config = new ServiceBusTriggeredEndpointConfiguration(EndpointName);
            var dbContextBuilder = new DbContextOptionsBuilder<MyDbContext>();
            dbContextBuilder.UseSqlServer(Environment.GetEnvironmentVariable("MyDbConnectionString2"));

            // should be in a UOW to dispose the context and so on
            config.AdvancedConfiguration.RegisterComponents(r => r.ConfigureComponent(() => new MyDbContext(dbContextBuilder.Options), DependencyLifecycle.InstancePerUnitOfWork));

            return config;
        });

        [FunctionName(EndpointName)]
        public static Task Run([ServiceBusTrigger(EndpointName)] Message message, ExecutionContext context, ILogger log)
        {
            return endpoint.Process(message, context, log);
        }
    }
}