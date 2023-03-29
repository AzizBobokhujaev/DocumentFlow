using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Api.Models.ViewModels;

public class EditOrderVm
{
    public int Id { get; set; }
    [Required]
    [Display(Name = "Ирсолкунанда")]
    public string Sender { get; set; } = null!;
    public int StatusId { get; set; }
    [Required]
    [Display(Name = "Муҳлати иҷрои ҳуҷҷат")]
    public List<SelectListItem> Users { get; set; } = new();
    public List<int> UserIds { get; set; } = new();
    public string? ResponseFilePath { get; set; }
    [Display(Name = "Тамдиди муҳлат")]
    public DateTime? ExtraDeadline { get; set; }
}