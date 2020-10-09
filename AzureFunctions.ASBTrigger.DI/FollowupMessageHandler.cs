using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region FollowupMessageHandler

public class FollowupMessageHandler : IHandleMessages<FollowupMessage>
{
    private readonly IMyService myService;
    private static readonly ILog Log = LogManager.GetLogger<FollowupMessageHandler>();

    public FollowupMessageHandler(IMyService myService)
    {
        this.myService = myService;
    }

    public Task Handle(FollowupMessage message, IMessageHandlerContext context)
    {
        Log.Warn($"Handling {nameof(FollowupMessage)} in {nameof(FollowupMessageHandler)}. Service says: {myService.SayHello()}");
        return Task.CompletedTask;
    }
}

#endregion