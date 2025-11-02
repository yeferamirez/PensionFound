using Amazon;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using PensionFund.Application.Configuration;
using PensionFund.Application.Context;
using PensionFund.Application.Interfaces;
using PensionFund.Domain.Interfaces.Repositories;
using PensionFund.Infrastructure.External.Extensions;
using PensionFund.Infrastructure.External.Notification;
using PensionFund.Infrastructure.External.Services;
using PensionFund.Infrastructure.External.Storage;

namespace PensionFund.Infrastructure.External;
public static class InfrastructureServiceRegister
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        CurrentEnvironment environment,
        StorageSettings storageSettings)
    {
        services.AddSingleton<StorageSettings>(storageSettings);

        services.AddRegisterStorage(storageSettings);

        if (!AppDomain.CurrentDomain.FriendlyName.Contains("ef") && environment.IsDevelopment() || environment.IsLocal())
        {
            services.AddScoped<DynamoDbInitializer>();

            using (var provider = services.BuildServiceProvider())
            {
                var initializer = provider.GetRequiredService<DynamoDbInitializer>();
                initializer.InitializeAsync().GetAwaiter().GetResult();
            }
        }

        return services;
    }

    public static void AddRegisterStorage(this IServiceCollection services, StorageSettings storageSettings)
    {
        var config = new AmazonDynamoDBConfig
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(storageSettings.Region),
            ServiceURL = storageSettings.ServiceURL
        };

        services.AddSingleton<IAmazonDynamoDB>(
            _ => new AmazonDynamoDBClient(
                storageSettings.AccessKeyId,
                storageSettings.SecretAccessKey,
                config));

        services.AddScoped(typeof(IDynamoRepository<>), typeof(DynamoRepository<>));

        services.AddScoped<IStorageService, StorageService>();

        services.AddScoped<IEmailRepository, EmailRepository>();
        services.AddScoped<ISmsRepository, SmsRepository>();
    }
}
