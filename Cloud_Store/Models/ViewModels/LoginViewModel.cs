using System.ComponentModel.DataAnnotations;

namespace Cloud_Store.Models.ViewModels;

public class LoginViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a username.")]
    public string? Username { get; set; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password.")]
    public string? Password { get; set; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password.")]
    public string? ConfirmPassword { get; set; }
    
    
}