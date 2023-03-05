namespace Api.Models.Entities;

public class Order
{
    public int Id { get; set; }
    public string DocumentNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string DecreeFilePath { get; set; } = null!;
    public string? ExecutionFilePath { get; set; } 
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = null!;
    public DateTime Deadline { get; set; }
    public virtual ICollection<User> Users { get; set; } = null!;
}