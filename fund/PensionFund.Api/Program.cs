using PensionFund.Api;
using PensionFund.Api.Constants;
using PensionFund.Api.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithCorrelationIdHeader(PensionFundConstants.Headers.CorrelationId)
    .CreateBootstrapLogger();

Log.Information("Starting web host in {Environment} for {Application}", builder.Environment.EnvironmentName, builder.Environment.ApplicationName);

try
{
    builder
        .UseSerilog();

    builder.Services
        .AddAspNetDependencies()
        .RegisterServices(builder.Configuration, builder.Environment);

    builder.Build()
        .ConfigureApp()
        .Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Web host terminated unexpectedly");
}
finally
{
    Log.Information("Web host shut down completed");
    Log.CloseAndFlush();
}
