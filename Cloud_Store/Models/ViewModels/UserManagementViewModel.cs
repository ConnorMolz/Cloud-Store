using Cloud_Store.Models.Entities;

namespace Cloud_Store.Models.ViewModels;

public class UserManagementViewModel
{
    // Data showing Section
    public UserAccount[]? UserAccounts { get; set; }
    
    // Data Input Section
    public UserAccount? CurrentUser { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
    
    // Controlling Section
    public bool InEditMode { get; set; }
    public bool InCreateMode {get; set;}
    public bool OpenModal { get; set; }
    
    // Constructor
    public UserManagementViewModel()
    {
        InEditMode = false;
        InCreateMode = false;
        OpenModal = false;
    }
    
}