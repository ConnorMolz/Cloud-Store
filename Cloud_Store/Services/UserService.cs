using Cloud_Store.Data;
using Cloud_Store.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Store.Services;

public interface IUserService
{
    Task<UserAccount[]> GetUserListAsync();
    Task<UserAccount> GetUserAsync(string username);
    Task AddUserAsync(UserAccount user);
}

public class UserService: IUserService
{
    private readonly CloudStoreContext _dbContext;
    
    public UserService(CloudStoreContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<UserAccount[]> GetUserListAsync()
    {
        return await _dbContext.UserAccounts.ToArrayAsync();
    }

    public Task<UserAccount> GetUserAsync(string username)
    {
        UserAccount? user = _dbContext.UserAccounts.FirstOrDefault(x => x.Username == username);
        if(user != null)
        {
            return Task.FromResult(user);
        }

        throw new Exception("No User is found with this username");
    }

    public async Task AddUserAsync(UserAccount user)
    {
        UserAccount? checkUser = _dbContext.UserAccounts.FirstOrDefault(x => x.Username == user.Username);
        if (checkUser == null)
        {
            _dbContext.UserAccounts.Add(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}