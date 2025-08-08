namespace ResilientConsumer.Models;

public record IncomingEvent<T>(int a, int b, T c);
