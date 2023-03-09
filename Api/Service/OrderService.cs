using Api.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Api.Service;

public class OrderService : IOrderService
{
    private readonly DocumentFlowDbContext _dbContext;

    public OrderService(DocumentFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ChangeStatusId()
    {
        var orders = await _dbContext.Orders
            .Where(order => order.Deadline <= DateTime.Now && (order.StatusId != 2 || order.ExecutionFilePath != null))
            .ToListAsync();
        foreach (var order in orders)
        {
            order.StatusId = 2;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Job completed");
        Console.ForegroundColor = ConsoleColor.White;
        _dbContext.Orders.UpdateRange(orders);
        await _dbContext.SaveChangesAsync();
    }
}