
using Microsoft.Extensions.Hosting;

namespace HqPocket.Extensions.Hosting.Services;

public abstract class TimedHostedService : IHostedService
{
    private Timer? _timer;
    protected abstract TimeSpan Period { get; set; }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, Period);
        return Task.CompletedTask;
    }

    protected abstract void DoWork(object? state);

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
