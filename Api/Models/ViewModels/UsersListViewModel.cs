using Api.Models.Entities;

namespace Api.Models.ViewModels;

public class UsersListViewModel
{
    public IEnumerable<User> Users { get; set; }
    public PagingInfo PagingInfo { get; set; }
    public string SearchString { get; set; }
}