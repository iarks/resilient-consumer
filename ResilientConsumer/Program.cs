using Rebus.Config;
using ResilientConsumer.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string internalReceiveQueueName = "ResilientConsumer_Rebus:Initial-Internal-Receive-Queue";

builder.Services.AddRebus(configure =>
{
    // setup a queue for handling incoming requests
    var configurer = configure
        .Logging(l => l.ColoredConsole())
        .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost:5672", internalReceiveQueueName)
            .SetPublisherConfirms(true));
    
    return configurer;
});

builder.Services.AutoRegisterHandlersFromAssemblyOf<RebusInitialQueueNotificationEnvelopeHandler>();

var app = builder.Build();

/*
app.Lifetime.ApplicationStarted.Register(async () =>
{
    await using var scope = app.Services.CreateAsyncScope();
    var bus = scope.ServiceProvider.GetRequiredService<IBus>();

    await bus.Advanced.Topics.Subscribe("x");
    await bus.Advanced.Topics.Subscribe("y");
});
*/

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();