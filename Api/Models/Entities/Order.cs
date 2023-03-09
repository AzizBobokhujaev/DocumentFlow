namespace Api.Models.Entities;

public class Order
{
    public int Id { get; set; }
    public string DocumentNumber { get; set; } = null!;
    public string DecreeName { get; set; } = null!;
    public string ImportDate { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string DecreeFilePath { get; set; } = null!;
    public string? ExecutionFilePath { get; set; } 
    public string? ExecutionDocumentName { get; set; }
    public DateTime? ExecutionFileCreatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public int StatusId { get; set; }
    public virtual Status Status { get; set; } = null!;
    public DateTime Deadline { get; set; }
    public virtual ICollection<User> Users { get; set; } = null!;
}