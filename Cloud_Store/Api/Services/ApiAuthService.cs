using Cloud_Store.Data;
using Cloud_Store.Services;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Store.Api.Services;

public interface IApiAuthService
{
    Task<bool> ValidateCredentialsAsync(string username, string password);
}

public class ApiAuthService : IApiAuthService
{
    private readonly IHashingService _hashingService;
    private readonly CloudStoreContext _context;

    public ApiAuthService(IHashingService hashingService, CloudStoreContext context)
    {
        _hashingService = hashingService;
        _context = context;
    }

    public async Task<bool> ValidateCredentialsAsync(string username, string password)
    {
        var user = await _context.UserAccounts.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return false;

        return _hashingService.VerifyPassword(password, user.Password);
    }
}