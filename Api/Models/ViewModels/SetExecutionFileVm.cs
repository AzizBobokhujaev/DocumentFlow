using System.ComponentModel.DataAnnotations;

namespace Api.Models.ViewModels;

public class SetExecutionFileVm
{
    public int Id { get; set; }
    [Display(Name = "Ҷавоби ҳуҷҷат")]
    public IFormFile ExecutionFile { get; set; } = null!;
}