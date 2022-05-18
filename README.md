# ASP.NET Core **Hosted Service** implementation to running background tasks

> In ASP.NET Core, background tasks can be implemented as hosted services. A hosted service is a class with background task logic that implements the IHostedService interface.
> They are an excellent way of running background tasks and is ideal if we need to update something that runs in the background that could effect all users (queued background tasks that run sequentially and so on).
> The hosted service is activated once at app startup and gracefully shut down at app shutdown.
> If an error is thrown during background task execution, Dispose should be called even if StopAsync isn't called.

### IHostedService interface

This interface defines two methods for objects that are managed by the host:

##### *StartAsync*

* StartAsync is called before and contains the logic to start the background task
* StartAsync should be limited to short running tasks because hosted services are run sequentially and no further services are started until the current runs to completion

##### *StopAsync*

* StopAsync is triggered when the host is performing a graceful shutdown and contains the logic to end the background task

### Implementation

1. First, we need to inherit the IHostedService interface. Within that, we must implement the StartAsync and StopAsync methods into our class:

```csharp
public class HostedService : IHostedService, IAsyncDisposable
{
    private Timer? timer;

    private void BackgroundWork(object? state) =>
        Console.WriteLine($"{DateTime.Now:T} - Start Background Work");

    public Task StartAsync(CancellationToken cancellationToken)
    {
        timer = new Timer(BackgroundWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine($"{DateTime.Now:T} - Stop Background Work");
        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        _ = timer?.DisposeAsync();
        return ValueTask.CompletedTask;
    }
}
```

2. Secondly, our service is registered in IHostBuilder.ConfigureServices (Program.cs) with the AddHostedService extension method:

```csharp
builder.Services.AddHostedService<BackgroundWork.Services.HostedService>();
```

### Demonstration

<div align="center">
    <img alt="demonstration" title="Background work demo" width="720em" height="auto" src="https://raw.githubusercontent.com/sharpist/BackgroundWork/master/demo.gif" />
</div>

