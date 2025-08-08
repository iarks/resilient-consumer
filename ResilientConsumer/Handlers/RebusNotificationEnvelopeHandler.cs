using Rebus.Bus;
using Rebus.Handlers;
using ResilientConsumer.Models;
using Wolverine;

namespace ResilientConsumer.Handlers;

public class RebusNotificationEnvelopeHandler: 
    IHandleMessages<NotificationServiceEnvelope<string>>,
    IHandleMessages<NotificationServiceEnvelope<IncomingEvent<string>>>,
    IHandleMessages<NotificationServiceEnvelope<IncomingEvent<decimal>>>
{
    private readonly IBus _bus;

    RebusNotificationEnvelopeHandler(IBus bus)
    {
        _bus = bus;
    }
    
    public Task Handle(NotificationServiceEnvelope<string> message)
    {
        Console.WriteLine($"Got message: {message.IncomingMessage}");
        return _bus.Advanced.Topics.Publish("x", message);
    }


    public Task Handle(NotificationServiceEnvelope<IncomingEvent<string>> message)
    {
        Console.WriteLine($"Got message: {message.IncomingMessage!.a}, {message.IncomingMessage!.b}, {message.IncomingMessage!.c}");
        return _bus.Advanced.Topics.Publish("x", message);
    }

    public Task Handle(NotificationServiceEnvelope<IncomingEvent<decimal>> message)
    {
        Console.WriteLine($"Got message: {message.IncomingMessage!.a}, {message.IncomingMessage!.b}, {message.IncomingMessage!.c}");
        return _bus.Advanced.Topics.Publish("y", message);
    }
}