using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PensionFund.Api.Extensions;

namespace PensionFund.Api.Filters;

public class AuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext actionContext)
    {
        var user = actionContext.HttpContext.User.Identity?.GetUserFromClaims();

        if (user == null)
        {
            actionContext.Result = new UnauthorizedResult();
            return;
        }
    }
}
