using ResilientConsumer.Models;
using Wolverine.Attributes;
using Wolverine.ErrorHandling;
using Wolverine.Runtime.Handlers;

namespace ResilientConsumer.Handlers;

[WolverineHandler]
public class NotificationHandlers
{
    private ConsoleColor _color = ConsoleColor.Yellow;

    private readonly ILogger<NotificationHandlers> _logger;
    
    public NotificationHandlers(ILogger<NotificationHandlers> logger)
    {
        _logger = logger;
    }
    
    public static void Configure(HandlerChain chain)
    {
        chain.OnException<ArgumentException>()
            .CustomAction((runtime, lifecycle, ex) =>
            {
                var logger = runtime.Services.GetRequiredService<ILogger<NotificationHandlers>>();
                Console.WriteLine($"Got exception at exception handler: {ex}");
                logger.LogError(ex, ex.Message);
                return ValueTask.FromException(ex);
            }, String.Empty, InvokeResult.Stop)
            .Then
            .MoveToErrorQueue();
    }
    
    public Task Handle(NotificationServiceEnvelope<string> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message at initial q handler: {message.IncomingMessage}");
        Console.ResetColor();
        return Task.CompletedTask;
    }


    public Task Handle(NotificationServiceEnvelope<IncomingEvent<string>> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message at initial q handler: {message.IncomingMessage!.a}, {message.IncomingMessage!.b}, {message.IncomingMessage!.c}");
        Console.ResetColor();
        throw new ArgumentNullException("Something went wrong");
    }

    public Task Handle(NotificationServiceEnvelope<IncomingEvent<decimal>> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message at initial q handler: {message.IncomingMessage!.a}, {message.IncomingMessage!.b}, {message.IncomingMessage!.c}");
        Console.ResetColor();
        return Task.CompletedTask;
    }
}