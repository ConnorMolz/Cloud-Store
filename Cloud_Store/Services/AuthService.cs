using System.Security.Claims;
using Cloud_Store.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Cloud_Store.Services;

public interface IAuthService
{
    Task<bool> AuthenticateAsync(string username, string password);
    Task LogoutAsync();
}

public class AuthService : IAuthService
{
    private readonly CloudStoreContext _dbContext;
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly NavigationManager _navigationManager;
    private HashingService _hashingService;

    public AuthService(
        CloudStoreContext dbContext,
        ProtectedSessionStorage sessionStorage,
        NavigationManager navigationManager)
    {
        _dbContext = dbContext;
        _sessionStorage = sessionStorage;
        _navigationManager = navigationManager;
        _hashingService = new HashingService();
    }

    public async Task<bool> AuthenticateAsync(string username, string password)
    {
        var userAccount = _dbContext.UserAccounts
            .FirstOrDefault(x => x.Username == username);

        if (userAccount == null || !_hashingService.VerifyPassword(password, userAccount.Password))
        {
            return false;
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userAccount.Username),
            new Claim(ClaimTypes.Role, userAccount.Role)
        };

        var identity = new ClaimsIdentity(claims, "CustomAuth");
        var user = new ClaimsPrincipal(identity);

        // Store the authentication state
        await _sessionStorage.SetAsync("auth", new AuthState
        {
            IsAuthenticated = true,
            Username = userAccount.Username,
            Role = userAccount.Role
        });

        return true;
    }

    public async Task LogoutAsync()
    {
        await _sessionStorage.DeleteAsync("auth");
        _navigationManager.NavigateTo("/login", true);
    }
}

public class AuthState
{
    public bool IsAuthenticated { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
