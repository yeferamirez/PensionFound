using PensionFund.Api.Constants;
using Serilog;

namespace PensionFund.Api.Logging;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder UseSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration)
        .Enrich.WithCorrelationIdHeader(PensionFundConstants.Headers.CorrelationId))
        .ConfigureAppConfiguration((c, b) => ConfigureApp(c, b));

        builder.Services.AddSerilog();

        return builder;
    }

    private static void ConfigureApp(HostBuilderContext context, IConfigurationBuilder builder)
    {

    }
}
