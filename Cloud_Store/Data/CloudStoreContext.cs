using Cloud_Store.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Store.Data;

public class CloudStoreContext: DbContext
{
    public CloudStoreContext(DbContextOptions<CloudStoreContext> options) : base(options)
    {
        
    }
    
    public DbSet<UserAccount> UserAccounts { get; set; }
}