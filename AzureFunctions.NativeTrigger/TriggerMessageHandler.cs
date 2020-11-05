using System.Threading.Tasks;
using AzureFunctions.NativeTrigger;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;

public class TriggerMessageHandler : IHandleMessages<TriggerMessage>
{
    private static readonly ILog Log = LogManager.GetLogger<TriggerMessageHandler>();

    private readonly MyDbContext myDbContext;

    public TriggerMessageHandler(MyDbContext myDbContext)
    {
        this.myDbContext = myDbContext;
    }

    public async Task Handle(TriggerMessage message, IMessageHandlerContext context)
    {
        Log.Warn($"Handling {nameof(TriggerMessage)} in {nameof(TriggerMessageHandler)}.");

        var any = await myDbContext.Users.AnyAsync();
        Log.Info($"Has users: {any}");
    }
}