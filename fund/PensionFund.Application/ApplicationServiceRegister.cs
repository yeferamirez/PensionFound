using FluentValidation;
using IdGen;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PensionFund.Application.Behaviors;
using PensionFund.Application.Security;
using PensionFund.Application.Security.Configuration;

namespace PensionFund.Application;

public static class ApplicationServiceRegister
{
    public static IServiceCollection AddPensionFundApplicationServices(
        this IServiceCollection services,
        JwtSettings jwtSettings)
    {
        services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegister).Assembly);
        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(ApplicationServiceRegister).Assembly));

        services.AddTrackingIdGenerator();

        return services;
    }

    public static IServiceCollection AddSharedApplicationServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IJwtGeneratorService, JwtGeneratorService>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

        return services;
    }

    private static void AddTrackingIdGenerator(this IServiceCollection services)
    {
        var structure = new IdStructure(35, 20, 8);
        var options = new IdGeneratorOptions(structure, new DefaultTimeSource(DateTime.UtcNow));

        services.AddSingleton<IdGeneratorOptions>(options);
    }
}
