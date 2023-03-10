using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Api.Models.ViewModels;

public class EditOrderVm
{
    public int Id { get; set; }
    [Required]
    [Display(Name = "Ирсолкунанда")]
    public string Title { get; set; } = null!;
    public int StatusId { get; set; }
    [Display(Name = "Ҷавоби ҳуҷҷат")]
    public string? ExecutionDocumentName { get; set; }
    public IFormFile? ResponseFile { get; set; }
    [Required]
    [Display(Name = "Муҳлати иҷрои ҳуҷҷат")]
    public DateTime Deadline { get; set; }
    public List<SelectListItem> Users { get; set; } = new();
    public List<int> UserIds { get; set; } = new();
    public string? ExecutionFilePath { get; set; }
    [Display(Name = "Тамдиди ҳуҷҷат")]
    public DateTime? ExtraDeadline { get; set; }
}