using Rebus.Config;
using ResilientConsumer.Handlers;
using Wolverine;
using Wolverine.ErrorHandling;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRebus(configure =>
{
    var configurer = configure
        .Logging(l => l.ColoredConsole())
        .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost:5672", "qqq").SetPublisherConfirms(true));
        
    return configurer;
});

builder.Services.AutoRegisterHandlersFromAssemblyOf<RebusNotificationEnvelopeHandler>();

const string prefix = "resilient-queue-internal:";



var app = builder.Build();

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