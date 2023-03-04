using Microsoft.AspNetCore.Mvc.Rendering;

namespace Api.Models.ViewModels;

public class CreateOrderVm
{
    public string DocumentNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public IFormFile DecreeFile { get; set; } = null!;
    public DateOnly Deadline { get; set; }
    public List<SelectListItem> Users { get; set; } = new();
    public List<int> UserIds { get; set; } = new(); 
}