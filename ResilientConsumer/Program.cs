using JasperFx;
using Microsoft.EntityFrameworkCore;
using ResilientConsumer.Controllers;
using ResilientConsumer.Handlers;
using ResilientConsumer.Models;
using ResilientConsumer.Persistence;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.ErrorHandling;
using Wolverine.Postgresql;
using Wolverine.Runtime.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

const string internalTransport = "wolverine_transport";

const string connectionString = "Host=localhost;Database=wolverine;Username=postgres;Password=postgres";

builder.Services.AddWolverine(configure =>
{
    /*
     * With this setting, if there exists 2 or more handers for the same message type,
     * each of them are in a separate thread
     */
    configure.MultipleHandlerBehavior = MultipleHandlerBehavior.Separated;

    /*
     * This is a special exception handler which will be invoked only when the postgres backplane is not available
     * This is to be used for disabling the main consumer
     */
    configure.OnException(x => x.GetType() == typeof(Npgsql.NpgsqlException) || x.GetType().Namespace!.Contains("Wolverine"))
        .CustomAction((runtime, envelopeLc, ex) =>
        {
            QueueNotifs.TimeOut = DateTime.UtcNow.AddMinutes(5);
            return ValueTask.CompletedTask;
        }, "Set the timeout value for which the consumer should ignore messages", InvokeResult.Stop);

    
    /*
     * Any exception that is moved to the error queue is also logged
     */
    configure.Policies.OnException(x => true)
        .MoveToErrorQueue().And((wolverineRuntime, context, ex) =>
        {
            // log here
            return ValueTask.CompletedTask;
        });

    /*
     * all local queues are considered durable,
     * which means that they are held in-memory or in the database before they are processed
     */
    configure.Policies.UseDurableLocalQueues();
    
    /*
     * This will enable postgres to be used for persisting messages before they are moved to the respective handlers
     */
    configure.PersistMessagesWithPostgresql(connectionString, internalTransport)
        .EnableMessageTransport(c =>
    {
        c.ConfigureSenders(s =>
        {
            s.CircuitBreaking(c =>
            {
                c.FailuresBeforeCircuitBreaks = 10;
                c.MaximumEnvelopeRetryStorage = 5;
                c.PingIntervalForCircuitResume =  TimeSpan.FromSeconds(5);
            });
        });
    });
    
    //configure.Policies.UseDurableLocalQueues();
    
    configure.Policies.AddMiddleware<Scoping>(x => x.MessageType == typeof(NotificationServiceEnvelope<>));
    
    // Registers the DbContext type in your IoC container, sets the DbContextOptions
    // lifetime to "Singleton" to optimize Wolverine usage, and also makes sure that
    // your Wolverine service has all the EF Core transactional middleware, saga support,
    // and storage operation helpers activated for this application
    configure.Services.AddDbContextWithWolverineIntegration<ConsumerDbContext>(x => x.UseNpgsql(connectionString));
});

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

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();

await app.RunJasperFxCommands(args);

//app.Run();