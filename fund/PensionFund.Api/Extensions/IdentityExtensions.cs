using PensionFund.Application.Exceptions;
using PensionFund.Application.Security;
using System.Security.Claims;
using System.Security.Principal;

namespace PensionFund.Api.Extensions;

public static class IdentityExtensions
{
    public static AuthenticatedUser? GetUserFromClaims(this IIdentity identity)
    {
        if (identity is not ClaimsIdentity claims || !identity.IsAuthenticated)
        {
            return null;
        }

        var id = claims.FindFirst("id")?.Value
                 ?? claims.FindFirst(ClaimTypes.NameIdentifier)?.Value
                 ?? throw new PensionFundException("Invalid token claims. Id not found.");

        var email = claims.FindFirst(ClaimTypes.Email)?.Value
                    ?? "localuser@localhost";

        var name = claims.FindFirst("name")?.Value
                    ?? claims.FindFirst(ClaimTypes.Name)?.Value
                    ?? "Local User";

        _ = int.TryParse(id, out var userId);

        return new AuthenticatedUser(
            userId,
            email,
            name);
    }
}

