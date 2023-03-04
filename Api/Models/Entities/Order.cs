namespace Api.Models.Entities;

public class Order
{
    public int Id { get; set; }
    public string DocumentNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string DecreeFilePath { get; set; } = null!;
    public string? ExecutionFilePath { get; set; } 
    public DateTime CreatedAt { get; set; }
    public virtual string Status { get; set; } = Entities.Status.InProgress.Name;
    public DateOnly Deadline { get; set; }
    public virtual ICollection<User> Users { get; set; } = null!;
}