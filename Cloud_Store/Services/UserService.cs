using Cloud_Store.Data;
using Cloud_Store.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Store.Services;

public interface IUserService
{
    Task<UserAccount[]> GetUserListAsync();
    Task<UserAccount> GetUserAsync(string username);
    Task AddUserAsync(UserAccount user);
    
    Task UpdateUserAsync(UserAccount user);
    Task ChangePasswordAsync(string username, string newPassword);
    Task DeleteUserAsync(string username);
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
    
    public async Task UpdateUserAsync(UserAccount user)
    {
        UserAccount? checkUser = _dbContext.UserAccounts.FirstOrDefault(x => x.Username == user.Username);
        if (checkUser != null)
        {
            checkUser.Username = user.Username;
            checkUser.Password = user.Password;
            checkUser.Role = user.Role;
            await _dbContext.SaveChangesAsync();
        }
    }
    
    public async Task ChangePasswordAsync(string username, string newPassword)
    {
        UserAccount? user = _dbContext.UserAccounts.FirstOrDefault(x => x.Username == username);
        if (user != null)
        {
            user.Password = newPassword;
            await _dbContext.SaveChangesAsync();
        }
    }
    public async Task DeleteUserAsync(string username)
    {
        UserAccount? user = _dbContext.UserAccounts.FirstOrDefault(x => x.Username == username);
        if (user != null)
        {
            _dbContext.UserAccounts.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}