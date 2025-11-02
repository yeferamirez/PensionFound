using PensionFund.Application.Security.Configuration;
using System.Security.Claims;

namespace PensionFund.Application.Security;

public interface IJwtGeneratorService
{
    public string BuildToken(
        JwtSettings jwtSettings,
        Claim[] claims);
}
