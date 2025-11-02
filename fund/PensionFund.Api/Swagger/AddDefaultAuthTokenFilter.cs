using Microsoft.OpenApi.Models;
using PensionFund.Application.Clients;
using PensionFund.Application.Security;
using PensionFund.Application.Security.Configuration;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Claims;

namespace PensionFund.Api.Swagger;

public class AddDefaultAuthTokenFilter : IDocumentFilter
{
    private readonly IJwtGeneratorService tokenService;
    private readonly JwtSettings jwtSettings;
    private readonly ClientsSettings clientsSettings;

    public AddDefaultAuthTokenFilter(
        IJwtGeneratorService tokenService,
        JwtSettings jwtSettings,
        ClientsSettings clientsSettings)
    {
        this.tokenService = tokenService;
        this.jwtSettings = jwtSettings;
        this.clientsSettings = clientsSettings;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var defaultToken = $"Bearer {this.tokenService.BuildToken(this.jwtSettings, GetClaims())}";

        if (swaggerDoc.Components.SecuritySchemes.ContainsKey("Bearer"))
        {
            swaggerDoc.Components.SecuritySchemes["Bearer"].Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            };

            swaggerDoc.Components.SecuritySchemes["Bearer"].Description = defaultToken;
        }
    }

    private Claim[] GetClaims()
    {
        var userId = clientsSettings.DefaultAdmin.Id;

        var claims = new List<Claim>
        {
            new Claim("id", userId.ToString()),
            new Claim("name", "Admin User"),
            new Claim(ClaimTypes.Email, "admin@local.test"),
        };

        return claims.ToArray();
    }
}
