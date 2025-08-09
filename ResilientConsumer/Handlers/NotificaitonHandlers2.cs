using ResilientConsumer.Models;
using Wolverine.Attributes;

namespace ResilientConsumer.Handlers;

[WolverineHandler]
public class WolverineInitialQueueHandler
{
    private ConsoleColor _color = ConsoleColor.Red;

    public Task Handle(NotificationServiceEnvelope<string> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message at initial q handler: {message.IncomingMessage}");
        Console.ResetColor();
        return Task.CompletedTask;
    }
    
    /*
    public Task Handle(NotificationServiceEnvelope<IncomingEvent<string>> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message at initial q handler: {message.IncomingMessage!.a}, {message.IncomingMessage!.b}, {message.IncomingMessage!.c}");
        Console.ResetColor();
        return Task.CompletedTask;
    }*/

    public Task Handle(NotificationServiceEnvelope<IncomingEvent<decimal>> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message at initial q handler: {message.IncomingMessage!.a}, {message.IncomingMessage!.b}, {message.IncomingMessage!.c}");
        Console.ResetColor();
        return Task.CompletedTask;
    }
}
