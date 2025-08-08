using System.Diagnostics;
using System.Net.Sockets;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;
using ResilientConsumer.Models;
using Wolverine;

namespace ResilientConsumer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMessageBus _messageBus;
    private readonly Random _random;
    private readonly IBus _bus;

    public HomeController(ILogger<HomeController> logger, /*IMessageBus messageBus,*/ IBus rebusBus)
    {
        _logger = logger;
//        _messageBus = messageBus;
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
        var a = _random.Next();
        var b = _random.Next();
        var c = _random.Next();
        var simulatedIncomingEvent = new NotificationServiceEnvelope<IncomingEvent<decimal>> { IncomingMessage = new IncomingEvent<decimal>(a, b, (decimal)c) };
        
        await _bus.Advanced.Routing.Send("qqq", simulatedIncomingEvent);
        
        //await _messageBus.EndpointFor("resilient-queue-internal:initial-consumer").InvokeAsync(simulatedIncomingEvent);

        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> PublishString()
    {
        var a = _random.Next();
        var b = _random.Next();
        var simulatedIncomingEvent = new NotificationServiceEnvelope<IncomingEvent<string>> { IncomingMessage = new IncomingEvent<string>(a, b, "hello world") };
        //await _messageBus.EndpointFor("resilient-queue-internal:initial-consumer").InvokeAsync(simulatedIncomingEvent);

        await _bus.Advanced.Routing.Send("qqq", simulatedIncomingEvent);
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> PublishConcrete()
    {
        var a = _random.Next();
        var b = _random.Next();
        var simulatedIncomingEvent = new NotificationServiceEnvelope<string> { IncomingMessage = "hello world" };
        //await _messageBus.EndpointFor("resilient-queue-internal:initial-consumer").SendAsync(simulatedIncomingEvent);

        await _bus.Advanced.Routing.Send("qqq", simulatedIncomingEvent);
        return View("Index");
    }
}

