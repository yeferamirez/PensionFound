using Ardalis.GuardClauses;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PensionFund.Api.Attributes;
using PensionFund.Api.Extensions;
using PensionFund.Api.Filters;
using PensionFund.Api.Mappers;
using PensionFund.Api.Middleware;
using PensionFund.Api.Swagger;
using PensionFund.Application;
using PensionFund.Application.Clients;
using PensionFund.Application.Configuration;
using PensionFund.Application.Consumers;
using PensionFund.Application.Context;
using PensionFund.Application.Security.Configuration;
using PensionFund.Infrastructure.EF;
using PensionFund.Infrastructure.External;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace PensionFund.Api;

public static class ApiServiceRegister
{
    public static IServiceCollection RegisterServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment)
    {
        var storageSettings = configuration.GetSection("StorageDb").Get<StorageSettings>() ?? Guard.Against.Null<StorageSettings>(null, "StorageDb", "Invalid Storage settings");
        var messagingSettings = configuration.GetSection("MessagingSettings").Get<MessagingSettings>() ?? Guard.Against.Null<MessagingSettings>(null, "Messaging", "Invalid Messaging settings");
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? Guard.Against.Null<JwtSettings>(null, "JwtSettings", "Invalid JwtSettings");
        var clientsSettings = configuration.GetSection("Clients").Get<ClientsSettings>() ?? Guard.Against.Null<ClientsSettings>(null, "Clients", "Invalid Clients settings");

        var env = new CurrentEnvironment(webHostEnvironment.EnvironmentName);
        services.AddSingleton<CurrentEnvironment>(env)
            .AddSingleton(jwtSettings)
            .AddSingleton(clientsSettings);

        services
            .AddPensionFundApplicationServices(jwtSettings)
            .AddHttpContextAccessor()
            .RegisterMassTransit(messagingSettings)
            .AddAutoMapper(Assembly.GetAssembly(typeof(PensionFundsProfile)));

        services.AddSingleton(TimeProvider.System);

        services
            .AddSharedApiServices()
            .AddJwtAuthentication(env, configuration, jwtSettings)
            .AddInfrastructureServices(env, storageSettings)
            .RegisterDatabase(configuration, env.IsDevelopment() || env.IsLocal());

        services.AddScoped<AuthorizationAttribute>();

        return services;
    }

    public static IServiceCollection AddSharedApiServices(
        this IServiceCollection services)
    {
        services.AddApplicationInsightsTelemetry();

        services.AddSharedApplicationServices();

        services.AddCors(options =>
        {
            options.AddPolicy(name: CorsPolicyBuilderExtensions.DefaultCorsPolicyName,
                policy => { policy.AddDefaultCors(); });
        });

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        CurrentEnvironment environment,
        IConfiguration configuration,
        JwtSettings jwtSettings)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    NameClaimType = "email"
                };
            });

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddAspNetDependencies(
        this IServiceCollection services)
    {
        services.AddSerilog();

        services.AddControllers(c =>
        {
            c.Filters.Add(typeof(BusinessExceptionAttribute));
        })
        .AddJsonOptions(c =>
        {
            c.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.AddCommonSwaggerOptions();
        })
            .AddCommonSwaggerServices<Program>();

        return services;
    }

    public static WebApplication ConfigureApp(
        this WebApplication app)
    {
        var env = new CurrentEnvironment(app.Environment.EnvironmentName);

        app.UseMiddleware<ExceptionsLoggerMiddleware>();

        app.UseCors(CorsPolicyBuilderExtensions.DefaultCorsPolicyName);

        app.UseSerilogRequestLogging();

        if (env.IsDevelopment() || env.IsLocal())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.ApplyMigrations();

            app.TraceAllSettings();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.MapGet("/", () => Results.Redirect("/swagger"));

        return app;
    }

    public static IServiceCollection AddCommonSwaggerServices<T>(
        this IServiceCollection services)
    {
        services.AddSwaggerExamplesFromAssemblyOf<T>();

        return services;
    }

    public static SwaggerGenOptions AddCommonSwaggerOptions(
        this SwaggerGenOptions options)
    {
        options.ExampleFilters();
        options.DocumentFilter<AddDefaultAuthTokenFilter>();

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });

        return options;
    }

    private static IServiceCollection RegisterMassTransit(
        this IServiceCollection services,
        MessagingSettings messagingSettings)
    {
        services.AddMassTransit(c =>
        {
            RegisterConsumers(c);

            c.UsingAmazonSqs((context, cfg) =>
            {
                cfg.Host(messagingSettings.Region, h =>
                {
                    h.AccessKey(messagingSettings.AccessKey);
                    h.SecretKey(messagingSettings.SecretKey);
                });

                RegisterEndpoints(cfg, context);
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    private static void RegisterConsumers(IBusRegistrationConfigurator busRegistrationConfigurator)
    {
        busRegistrationConfigurator.AddConsumer<NotifySubscribtedConsumer>();
    }

    private static void RegisterEndpoints<T>(IBusFactoryConfigurator<T> cfg, IBusRegistrationContext context) where T : IReceiveEndpointConfigurator
    {
        cfg.ReceiveEndpoint(NotifySubscribtedConsumer.EndpointName, e =>
        {
            e.UseMessageRetry(rc => rc.Interval(3, TimeSpan.FromSeconds(1)));
            e.ConfigureConsumer<NotifySubscribtedConsumer>(context);
        });
    }
}
