using Wolverine;

namespace ResilientConsumer.Handlers;

public class Scoping
{
    private readonly ILogger _logger;
    private IDisposable _logScope;
    public Scoping(ILogger logger)
    {
        _logger = logger;
    }

    public void Before()
    {
        _logScope = _logger.BeginScope("Scoping");
    }

    public void Finally(ILogger logger, Envelope envelope)
    {
        _logScope?.Dispose();
    }
}