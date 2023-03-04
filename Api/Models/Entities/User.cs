using Microsoft.AspNetCore.Identity;

namespace Api.Models.Entities;

public class User : IdentityUser<int>
{
    public virtual ICollection<Order> Orders { get; set; } = null!;
}