using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Api.Models.ViewModels;

public class EditOrderVm
{
    public int Id { get; set; }
    [Required]
    [Display(Name = "Мавзуъ")]
    public string Title { get; set; } = null!;
    public int StatusId { get; set; }
    public string? ExecutionDocumentName { get; set; }
    public IFormFile? ResponseFile { get; set; }
    [Required]
    [Display(Name = "Муҳлоти иҷроиш")]
    public DateTime Deadline { get; set; }
    public List<SelectListItem> Users { get; set; } = new();
    public List<int> UserIds { get; set; } = new();
    public string? ExecutionFilePath { get; set; }
}