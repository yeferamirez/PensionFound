using Microsoft.IdentityModel.Tokens;
using PensionFund.Application.Security.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PensionFund.Application.Security;
public class JwtGeneratorService : IJwtGeneratorService
{
    private readonly TimeProvider timeProvider;

    public JwtGeneratorService(TimeProvider timeProvider)
    {
        this.timeProvider = timeProvider;
    }

    public string BuildToken(
        JwtSettings jwtSettings,
        Claim[] claims)
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var expiration = now.AddMinutes(jwtSettings.ExpirationTimeInMinutes);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience ?? jwtSettings.Issuer,
            claims: claims,
            notBefore: now,
            expires: expiration,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
