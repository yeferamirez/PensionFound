using Microsoft.AspNetCore.Cors.Infrastructure;

namespace PensionFund.Api.Extensions;

public static class CorsPolicyBuilderExtensions
{
    public const string DefaultCorsPolicyName = "PensionFundCorsPolicy";

    public static void AddDefaultCors(this CorsPolicyBuilder corsBuilder)
    {
        corsBuilder
            .WithOrigins([])
            .WithHeaders("Authorization", "Content-Type", "Accept", "X-Requested-With", "X-HTTP-Method-Override")
            .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS");
    }
}
