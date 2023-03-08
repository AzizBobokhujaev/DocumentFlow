using Api.Service;

namespace Api.ScheduleWorkers;

public class ScheduleWorker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer = null!;
    
    public ScheduleWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(OrderStatusWorker, null, TimeSpan.Zero, TimeSpan.FromHours(6));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }
    
    private async void OrderStatusWorker(object? state)
    {
        using var scope = _serviceProvider.CreateScope();
        var accessService = scope.ServiceProvider.GetRequiredService<IOrderService>();
        await accessService.ChangeStatusId();
    }
}