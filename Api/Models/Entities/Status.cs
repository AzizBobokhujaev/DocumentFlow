using Api.Configure;

namespace Api.Models.Entities;

public class Status : Enumeration
{
    public static readonly Status Done = new(1, "Иҷро шуд");
    public static readonly Status NotDone = new(2, "Иҷро нашуд");
    public static readonly Status InProgress = new(3, "Дар ҳолати иҷроиш");
    
    protected Status(int id, string name) : base(id, name)
    {
    }
}