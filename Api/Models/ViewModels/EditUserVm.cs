using System.ComponentModel.DataAnnotations;

namespace Api.Models.ViewModels;

public class EditUserVm
{
    public int Id { get; set; }
    [Required] 
    [Display(Name = "Ному насаби истифодабаранда")] 
    public string UserName { get; set; } = null!;
    [Required]
    [Display(Name = "Рақами телефон")]
    public string PhoneNumber { get; set; } = null!;
}