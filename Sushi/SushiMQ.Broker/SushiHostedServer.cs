using Microsoft.Extensions.Hosting;
using SushiMQ.Broker.Interfaces;

namespace SushiMQ.Broker;

public class SushiHostedServer : IHostedService
{
    private ISushiListener _listener;
    
    public SushiHostedServer(ISushiListener listener)
    {
        _listener = listener;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _listener.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}