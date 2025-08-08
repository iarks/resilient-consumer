using JasperFx.Core;
using ResilientConsumer.Models;
using Wolverine;
using Wolverine.ErrorHandling;
using Wolverine.Runtime.Handlers;

namespace ResilientConsumer.Handlers;

public class MyMessageHandler
{
    private readonly ILogger<MyMessageHandler> _logger;

    public MyMessageHandler(ILogger<MyMessageHandler> logger)
    {
        _logger = logger;
    }

    public static void Configure(HandlerChain chain, ILogger<MyMessageHandler> logger)
    {
        chain.OnException<Exception>()
            //.RetryWithCooldown(5.Minutes(), 10.Seconds(), 20.Seconds(), 40.Seconds())
            //.Then
            //.Requeue(3)
            //.Then
            .CustomAction((runtime, envelopeLifecycle, exception) =>
            {
                logger.LogError(exception, exception.Message);
                return ValueTask.CompletedTask;
            }, "", InvokeResult.TryAgain)
            .Then
            .MoveToErrorQueue();
    }

    public async Task Handle(IncomingEvent<decimal> message, IMessageContext context)
    {
        Console.WriteLine($"Got message: {message.a}, {message.b}, {message.c}");
        await Task.Delay(100);
        throw new Exception();
    }

    public async Task Handle(IncomingEvent<string> message, IMessageContext context)
    {
        Console.WriteLine($"Got message: {message.a}, {message.b}, {message.c}");
        await Task.Delay(100);
    }
}
