namespace Api.Models.Entities;

public class Order
{
    public int Id { get; set; }
    // Р/т
    public string Rt { get; set; } = null!;
    // Санаи воридшавии хуччат
    public string ImportDate { get; set; } = null!;
    // № ва шохиси хуччат
    public string DocumentName { get; set; } = null!;
    public string DocumentRealName { get; set; } = null!;
    public string DocumentFilePath { get; set; } = null!;
    // Ирсолкунанда
    public string Sender { get; set; } = null!;
    // Иҷрокунанда
    public virtual ICollection<User> Users { get; set; } = null!;
    // муҳлати иҷроиш
    public DateTime Deadline { get; set; }
    // тамдиди ҳуҷҷат
    public DateTime? ExtraDeadline { get; set; }
    // Ҷавоби ҳуҷҷат
    public string? ResponseDocumentName { get; set; }
    public string? ResponseRealDocumentName { get; set; }
    public string? ResponseFilePath { get; set; }
    public DateTime? ResponseFileCreatedAt { get; set; }
    // Иҷроиш (статус)
    public int StatusId { get; set; }
    public virtual Status Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}