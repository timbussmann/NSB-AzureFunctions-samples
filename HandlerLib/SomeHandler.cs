using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace HandlerLib
{
    public class SomeHandler : IHandleMessages<object>
    {
        private static ILog logger = LogManager.GetLogger<SomeHandler>();

        public Task Handle(object message, IMessageHandlerContext context)
        {
            logger.Info($"{nameof(SomeHandler)} was invoked for message {message.GetType()}");
            return Task.CompletedTask;
        }
    }
}