using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class TriggerMessageHandler : IHandleMessages<TriggerMessage>
{
    private static readonly ILog Log = LogManager.GetLogger<TriggerMessageHandler>();
    private readonly IMyService myService;

    public TriggerMessageHandler(IMyService myService)
    {
        this.myService = myService;
    }

    public Task Handle(TriggerMessage message, IMessageHandlerContext context)
    {
        Log.Warn($"Handling {nameof(TriggerMessage)} in {nameof(TriggerMessageHandler)}. Service says: {myService.SayHello()}");

        return Task.CompletedTask;
    }
}