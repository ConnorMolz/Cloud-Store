using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Cloud_Store.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthStateProvider(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var authStateResult = await _sessionStorage.GetAsync<AuthState>("auth");
            var authState = authStateResult.Success ? authStateResult.Value : null;

            if (authState == null)
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }

            var claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, authState.Username),
                    new Claim(ClaimTypes.Role, authState.Role)
                }, "CustomAuth"));

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch
        {
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }
    }

    public async Task UpdateAuthenticationState(AuthState? authState)
    {
        if (authState != null)
        {
            await _sessionStorage.SetAsync("auth", authState);
        }
        else
        {
            await _sessionStorage.DeleteAsync("auth");
        }

        var identity = authState != null
            ? new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, authState.Username),
                new Claim(ClaimTypes.Role, authState.Role)
            }, "CustomAuth")
            : new ClaimsIdentity();

        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }
}