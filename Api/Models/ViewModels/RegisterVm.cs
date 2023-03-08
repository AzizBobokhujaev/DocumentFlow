using System.ComponentModel.DataAnnotations;

namespace Api.Models.ViewModels;

public class RegisterVm
{
    [Required] 
    [Display(Name = "Ному насаби истифодабаранда")] 
    public string UserName { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Display(Name = "Почтаи электронӣ")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Рақами телефон")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required] 
    [Display(Name = "Рамз")] 
    public string Password { get; set; } = string.Empty;
    
    [Display(Name = "Тасдиқи рамз")]
    [Compare("Password", ErrorMessage = "Мутобиқати рамз")]
    public string ConfirmPassword { get; set; } = string.Empty;
}