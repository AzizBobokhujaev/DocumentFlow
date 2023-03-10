using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Api.Models.ViewModels;

public class CreateOrderVm
{
    [Required]
    [Display(Name = "Р/т")]
    public string DocumentNumber { get; set; } = null!;
    [Required]
    [Display(Name = "Санаи воридшавии ҳуҷҷат")]
    public string ImportDate { get; set; } = null!;
    [Required]
    [Display(Name = "№ ва шохиси ҳуҷҷатҳои воридшуда")]
    public string DecreeName { get; set; } = null!;
    [Required]
    [Display(Name = "Ирсолкунанда")]
    public string Title { get; set; } = null!;
    [Required]
    [Display(Name = "Ҳуҷҷатро интихоб кунед")]
    public IFormFile DecreeFile { get; set; } = null!;
    [DataType(DataType.DateTime)]
    [Display(Name = "Муҳлати иҷрои ҳуҷҷат")]
    public DateTime Deadline { get; set; }
    [Display(Name = "Амри хаттӣ ё ба кӣ фиристода шудани ҳуҷҷат")]
    public List<SelectListItem> Users { get; set; } = new();
    public List<int> UserIds { get; set; } = new(); 
}