namespace BackgroundWork.Services;

public class HostedService : IHostedService, IAsyncDisposable
{
    //private readonly IServiceProvider serviceProvider;
    private Timer? timer;

    private void BackgroundWork(object? state) =>
        Console.WriteLine($"{DateTime.Now:T} - Start Background Work");

    public Task StartAsync(CancellationToken cancellationToken)
    {
        //how we can run an infinite task
        //Task.Run(async () =>
        //{
        //    while (!cancellationToken.IsCancellationRequested)
        //    {
        //        how we can use dependency injection
        //        using (var scope = serviceProvider.CreateScope())
        //        {
        //            var myScopedService = scope.ServiceProvider.GetRequiredService<IMyScopedService>();
        //        }

        //        await Task.Delay(new TimeSpan(0, 1, 0));
        //    }
        //});

        timer = new Timer(BackgroundWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine($"{DateTime.Now:T} - Stop Background Work");
        return Task.CompletedTask;
    }

    //public HostedService(IServiceProvider serviceProvider) =>
    //    this.serviceProvider = serviceProvider;

    public ValueTask DisposeAsync()
    {
        _ = timer?.DisposeAsync();
        return ValueTask.CompletedTask;
    }
}
