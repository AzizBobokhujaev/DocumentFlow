using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Api.Models.ViewModels;

public class CreateOrderVm
{
    [Required]
    [Display(Name = "Рақами ҳуҷҷат")]
    public string DocumentNumber { get; set; } = null!;
    [Required]
    [Display(Name = "Мавзуъ")]
    public string Title { get; set; } = null!;
    [Required]
    [Display(Name = "Ҳуҷҷатро интихоб кунед")]
    public IFormFile DecreeFile { get; set; } = null!;
    [DataType(DataType.DateTime)]
    [Display(Name = "Муҳлоти иҷроиш")]
    public DateTime Deadline { get; set; }
    [Display(Name = "Иҷрокунандаҳо")]
    public List<SelectListItem> Users { get; set; } = new();
    public List<int> UserIds { get; set; } = new(); 
}