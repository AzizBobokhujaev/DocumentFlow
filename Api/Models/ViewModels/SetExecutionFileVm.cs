namespace Api.Models.ViewModels;

public class SetExecutionFileVm
{
    public int Id { get; set; }
    public IFormFile ExecutionFile { get; set; } = null!;
}