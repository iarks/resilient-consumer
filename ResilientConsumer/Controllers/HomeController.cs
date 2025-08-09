using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResilientConsumer.Models;
using Wolverine;

namespace ResilientConsumer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Random _random;
    private readonly IMessageBus _bus;

    string internalReceiveQueueName = "ResilientConsumer-internal-receive-queue";
    
    public HomeController(ILogger<HomeController> logger, IMessageBus rebusBus)
    {
        _logger = logger;
        _bus = rebusBus;
        _random = new Random();
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public async Task<IActionResult> PublishDecimal()
    {
        if (!QueueNotifs.ShouldProcess)
        {
            throw new InvalidOperationException("Not allowed. Please try after some time");
        }
        
        var a = _random.Next();
        var b = _random.Next();
        var c = _random.Next();
        var simulatedIncomingEvent = new NotificationServiceEnvelope<IncomingEvent<decimal>> { IncomingMessage = new IncomingEvent<decimal>(a, b, (decimal)c) };
        await _bus.PublishAsync(simulatedIncomingEvent);
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> PublishString()
    {
        if (!QueueNotifs.ShouldProcess)
        {
            throw new InvalidOperationException("Not allowed. Please try after some time");
        }
        
        var a = _random.Next();
        var b = _random.Next();
        var simulatedIncomingEvent = new NotificationServiceEnvelope<IncomingEvent<string>> { IncomingMessage = new IncomingEvent<string>(a, b, "hello world") };
        await _bus.PublishAsync(simulatedIncomingEvent);
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> PublishConcrete()
    {
        if (!QueueNotifs.ShouldProcess)
        {
            throw new InvalidOperationException("Not allowed. Please try after some time");
        }
        
        var a = _random.Next();
        var b = _random.Next();
        var simulatedIncomingEvent = new NotificationServiceEnvelope<string> { IncomingMessage = "hello world" };
        await _bus.PublishAsync(simulatedIncomingEvent);
        return View("Index");
    }
}

