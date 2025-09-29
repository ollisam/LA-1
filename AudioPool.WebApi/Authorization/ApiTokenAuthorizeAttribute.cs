using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace AudioPool.WebApi.Authorization;

/// <summary>
/// Custom API token authorization attribute.
/// Requires an HTTP header 'api-token' with a valid token value.
/// </summary>
public class ApiTokenAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private const string HeaderName = "api-token";
    // Hard-coded token as per assignment requirements
    private const string ValidToken = "uss";

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var headers = context.HttpContext.Request.Headers;
        if (!headers.TryGetValue(HeaderName, out StringValues provided) || StringValues.IsNullOrEmpty(provided))
        {
            context.Result = new UnauthorizedObjectResult(new { error = "Missing api-token header" });
            return Task.CompletedTask;
        }

        if (!string.Equals(provided.ToString(), ValidToken, StringComparison.Ordinal))
        {
            context.Result = new UnauthorizedObjectResult(new { error = "Invalid api-token" });
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}
