using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.NativeTrigger
{
    public class AzureServiceBusNativeTriggeredFunction
    {
        private readonly MyDbContext myDbContext;

        public AzureServiceBusNativeTriggeredFunction(MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }

        [FunctionName("Function1")]
        public async Task Run([ServiceBusTrigger("asbtriggerqueue", Connection = "AzureWebJobsServiceBus")] Message message, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {Encoding.UTF8.GetString(message.Body)}");

            var any = await myDbContext.Users.AnyAsync();
        }
    }
}