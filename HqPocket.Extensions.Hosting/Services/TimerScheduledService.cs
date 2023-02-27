
using Microsoft.Extensions.Hosting;

namespace HqPocket.Extensions.Hosting.Services;

public abstract class TimerScheduledService : BackgroundService
{
    private readonly PeriodicTimer _periodicTimer;
    protected TimerScheduledService(TimeSpan period)
    {
        _periodicTimer = new PeriodicTimer(period);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(await _periodicTimer.WaitForNextTickAsync(stoppingToken))
        {
            await ExecuteInternal(stoppingToken);
        }
    }

    protected abstract Task ExecuteInternal(CancellationToken stoppingToken);

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _periodicTimer.Dispose();
        return base.StopAsync(cancellationToken);
    }
}
