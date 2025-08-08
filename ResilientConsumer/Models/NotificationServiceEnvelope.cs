namespace ResilientConsumer.Models;

public class NotificationServiceEnvelope<T>
{
    public T? IncomingMessage { get; init; } = default;
}
