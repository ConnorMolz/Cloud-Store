using Cloud_Store.Api.Services;
using Cloud_Store.Services;

namespace Cloud_Store.Api.Middleware;

public class BasicAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;
    private const string REALM = "Cloud_Store";

    public BasicAuthMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Response.StatusCode = 401;
            context.Response.Headers.Add("WWW-Authenticate", $"Basic realm=\"{REALM}\"");
            return;
        }

        using (var scope = _serviceProvider.CreateScope())
        {
            var apiAuthService = scope.ServiceProvider.GetRequiredService<IApiAuthService>();

            var authHeader = context.Request.Headers["Authorization"].ToString();
            var credentials = GetCredentialsFromHeader(authHeader);

            if (await apiAuthService.ValidateCredentialsAsync(credentials.username, credentials.password))
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = 401;
                context.Response.Headers.Add("WWW-Authenticate", $"Basic realm=\"{REALM}\"");
            }
        }
    }

    private (string username, string password) GetCredentialsFromHeader(string authHeader)
    {
        try
        {
            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            var decodedCredentials = Convert.FromBase64String(encodedCredentials);
            var credentialString = System.Text.Encoding.UTF8.GetString(decodedCredentials);
            var credentials = credentialString.Split(':', 2);
            return (credentials[0], credentials[1]);
        }
        catch
        {
            return (string.Empty, string.Empty);
        }
    }
}