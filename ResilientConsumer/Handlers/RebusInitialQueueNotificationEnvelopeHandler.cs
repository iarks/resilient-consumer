using System.Globalization;
using Rebus.Bus;
using Rebus.Handlers;
using ResilientConsumer.Models;

namespace ResilientConsumer.Handlers;

// these handlers should execute from the queue
public class RebusInitialQueueNotificationEnvelopeHandler: 
    IHandleMessages<NotificationServiceEnvelope<string>>,
    IHandleMessages<NotificationServiceEnvelope<IncomingEvent<string>>>,
    IHandleMessages<NotificationServiceEnvelope<IncomingEvent<decimal>>>
{
    private readonly IBus _bus;
    private ConsoleColor _color = ConsoleColor.Yellow;
    public RebusInitialQueueNotificationEnvelopeHandler(IBus bus)
    {
        _bus = bus;
    }
    
    public Task Handle(NotificationServiceEnvelope<string> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message at initial q handler: {message.IncomingMessage}");
        Console.ResetColor();
        return _bus.Advanced.Topics.Publish("x", message);
    }


    public Task Handle(NotificationServiceEnvelope<IncomingEvent<string>> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message at initial q handler: {message.IncomingMessage!.a}, {message.IncomingMessage!.b}, {message.IncomingMessage!.c}");
        Console.ResetColor();
        return _bus.Advanced.Topics.Publish("x", message);
    }

    public Task Handle(NotificationServiceEnvelope<IncomingEvent<decimal>> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message at initial q handler: {message.IncomingMessage!.a}, {message.IncomingMessage!.b}, {message.IncomingMessage!.c}");
        Console.ResetColor();
        return _bus.Advanced.Topics.Publish("y", message);
    }
}

// these handlers should execute from the messages coming in only from the topic, even though they have the same message type
public class RebusTopicNotificationEnvelopeHandler : IHandleMessages<NotificationServiceEnvelope<string>>,
    IHandleMessages<NotificationServiceEnvelope<IncomingEvent<string>>>,
    IHandleMessages<NotificationServiceEnvelope<IncomingEvent<decimal>>>
{
    private ConsoleColor _color = ConsoleColor.Cyan;
    
    public Task Handle(NotificationServiceEnvelope<string> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message at topic consumer 1: {message.IncomingMessage}");
        Console.ResetColor();
        return Task.CompletedTask;
    }


    public Task Handle(NotificationServiceEnvelope<IncomingEvent<string>> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message at topic consumer 1: {message.IncomingMessage!.a}, {message.IncomingMessage!.b}, {message.IncomingMessage!.c}");
        Console.ResetColor();
        return Task.CompletedTask;
    }

    public Task Handle(NotificationServiceEnvelope<IncomingEvent<decimal>> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message at topic consumer 1: {message.IncomingMessage!.a}, {message.IncomingMessage!.b}, {message.IncomingMessage!.c}");
        Console.ResetColor();
        return Task.CompletedTask;
    }
}


// these handlers should execute from the messages coming in from the topic, even though they have the same message type
public class RebusTopicNotificationEnvelopeHandler2 : IHandleMessages<NotificationServiceEnvelope<string>>,
    IHandleMessages<NotificationServiceEnvelope<IncomingEvent<string>>>,
    IHandleMessages<NotificationServiceEnvelope<IncomingEvent<decimal>>>
{
    private ConsoleColor _color = ConsoleColor.Magenta;
    
    public Task Handle(NotificationServiceEnvelope<string> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message at topic consumer 2: {message.IncomingMessage}");
        Console.ResetColor();
        return Task.CompletedTask;
    }


    public Task Handle(NotificationServiceEnvelope<IncomingEvent<string>> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message at topic consumer 2: {message.IncomingMessage!.a}, {message.IncomingMessage!.b}, {message.IncomingMessage!.c}");
        Console.ResetColor();
        return Task.CompletedTask;
    }

    public Task Handle(NotificationServiceEnvelope<IncomingEvent<decimal>> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message at topic consumer 2: {message.IncomingMessage!.a}, {message.IncomingMessage!.b}, {message.IncomingMessage!.c}");
        Console.ResetColor();
        return Task.CompletedTask;
    }
}
