namespace Api.Models.ViewModels;

public class EditUserVm
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}