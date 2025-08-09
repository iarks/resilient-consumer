using ImTools;
using JasperFx;
using Wolverine;
using Wolverine.ErrorHandling;
using Wolverine.Postgresql;
using Wolverine.RabbitMQ;
using Wolverine.RDBMS.Transport;
using Wolverine.Runtime.Handlers;
using Wolverine.Runtime.WorkerQueues;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

const string internalTransport = "wolverine_transport";

builder.Services.AddWolverine(configure =>
{
    configure.PersistMessagesWithPostgresql("Host=localhost;Database=wolverine;Username=postgres;Password=postgres",
        internalTransport);
    
    configure.OnException<Npgsql.NpgsqlException>()
        .CustomAction((runtime, envelopeLc, ex) =>
        {
            runtime.Agents.AllRunningAgentUris();
            return ValueTask.CompletedTask;
        }, "", InvokeResult.Stop);
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