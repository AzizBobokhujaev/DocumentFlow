using Api.Models.Entities;

namespace Api.Models.ViewModels;

public class OrdersListViewModel
{
    public IEnumerable<Order> Orders { get; set; }
    public PagingInfo PagingInfo { get; set; }
    public string SearchString { get; set; }
}