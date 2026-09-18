using Serilog;
using VetCommission.Worker;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddSerilog((services, configuration) => configuration
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());
    builder.Services.AddWorkerComposition(builder.Configuration);

    var host = builder.Build();
    host.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "O Worker foi encerrado inesperadamente.");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
