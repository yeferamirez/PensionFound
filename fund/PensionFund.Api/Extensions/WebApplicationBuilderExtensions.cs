namespace PensionFund.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void TraceAllSettings(this WebApplication app)
    {
        var configurations = app.Configuration;
        var logger = app.Services.GetRequiredService<ILogger<WebApplication>>();

        foreach (var configuration in configurations.AsEnumerable())
        {
            logger.LogWarning("Key: {Key}, Value: {Value}", configuration.Key, configuration.Value);
        }
    }
}
