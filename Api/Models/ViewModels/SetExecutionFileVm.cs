using System.ComponentModel.DataAnnotations;

namespace Api.Models.ViewModels;

public class SetExecutionFileVm
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Номи Ҷавоби ҳуҷҷат")]
    public string ExecutionDocumentName { get; set; } = null!;
    [Required]
    [Display(Name = "Ҷавоби ҳуҷҷат")]
    public IFormFile ExecutionFile { get; set; } = null!;
}