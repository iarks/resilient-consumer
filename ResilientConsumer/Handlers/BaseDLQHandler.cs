using Wolverine;
using ResilientConsumer.Models;

namespace ResilientConsumer.Handlers;

public abstract class BaseDLQHandler<T>
{
    private readonly ILogger _logger;

    public BaseDLQHandler(ILogger logger)
    {
        _logger = logger;
    }

    public async Task Handle(T message, Envelope envelope, IMessageContext context)
    {
        await Task.Delay(100);
    }
}

public class DLQHandler:BaseDLQHandler<IncomingEvent<decimal>>
{
    public DLQHandler(ILogger<DLQHandler> logger):base(logger)
    {
    }
}
