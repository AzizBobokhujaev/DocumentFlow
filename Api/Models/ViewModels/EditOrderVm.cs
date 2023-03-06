using Microsoft.AspNetCore.Mvc.Rendering;

namespace Api.Models.ViewModels;

public class EditOrderVm
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int StatusId { get; set; }
    public IFormFile? ResponseFile { get; set; }
    public DateTime Deadline { get; set; }
    public List<SelectListItem> Users { get; set; } = new();
    public List<int> UserIds { get; set; } = new();
}