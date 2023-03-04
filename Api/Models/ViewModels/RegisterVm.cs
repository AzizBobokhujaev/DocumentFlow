using System.ComponentModel.DataAnnotations;

namespace Api.Models.ViewModels;

public class RegisterVm
{
    [Required] 
    [Display(Name = "Логин")] 
    public string UserName { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Display(Name = "Почтаи электроин")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Номер телефона")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required] 
    [Display(Name = "Пароль")] 
    public string Password { get; set; } = string.Empty;
    
    [Display(Name = "Подтверждение пароля")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; } = string.Empty;
}