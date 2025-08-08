using System.Globalization;
using Rebus.Bus;
using Rebus.Handlers;
using ResilientConsumer.Models;
using Wolverine;

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
        Console.WriteLine($"Got message: {message.IncomingMessage}");
        Console.ResetColor();
        return _bus.Advanced.Topics.Publish("x", message);
    }


    public Task Handle(NotificationServiceEnvelope<IncomingEvent<string>> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message: {message.IncomingMessage!.a}, {message.IncomingMessage!.b}, {message.IncomingMessage!.c}");
        Console.ResetColor();
        return _bus.Advanced.Topics.Publish("x", message);
    }

    public Task Handle(NotificationServiceEnvelope<IncomingEvent<decimal>> message)
    {
        Console.ForegroundColor = _color;
        Console.WriteLine($"Got message: {message.IncomingMessage!.a}, {message.IncomingMessage!.b}, {message.IncomingMessage!.c}");
        Console.ResetColor();
        return _bus.Advanced.Topics.Publish("y", message);
    }
}

/*
// these handlers should execute from the topic
public class RebusTopicNotificationEnvelopeHandler : IHandleMessages<NotificationServiceEnvelope<string>>,
    IHandleMessages<NotificationServiceEnvelope<IncomingEvent<string>>>,
    IHandleMessages<NotificationServiceEnvelope<IncomingEvent<decimal>>>
{
    private readonly IBus _bus;

    RebusTopicNotificationEnvelopeHandler(IBus bus)
    {
        _bus = bus;
    }
    
    public Task Handle(NotificationServiceEnvelope<string> message)
    {
        Console.WriteLine($"Got message: {message.IncomingMessage}");
        return Task.CompletedTask;
    }


    public Task Handle(NotificationServiceEnvelope<IncomingEvent<string>> message)
    {
        Console.WriteLine($"Got message: {message.IncomingMessage!.a}, {message.IncomingMessage!.b}, {message.IncomingMessage!.c}");
        return Task.CompletedTask;
    }

    public Task Handle(NotificationServiceEnvelope<IncomingEvent<decimal>> message)
    {
        Console.WriteLine($"Got message: {message.IncomingMessage!.a}, {message.IncomingMessage!.b}, {message.IncomingMessage!.c}");
        return Task.CompletedTask;
    }
}
*/