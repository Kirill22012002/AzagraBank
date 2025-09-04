using System.ComponentModel.DataAnnotations;

namespace AzagraBank.IdentityServer.Models;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Password not assert")]
    [Display(Name = "Accept password")]
    public string PasswordConfirm { get; set; }
}
