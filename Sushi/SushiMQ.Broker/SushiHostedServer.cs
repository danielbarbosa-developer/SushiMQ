using Microsoft.Extensions.Hosting;
using SushiMQ.Broker.Interfaces;
using SushiMQ.Engine;

namespace SushiMQ.Broker;

public class SushiHostedServer : IHostedService
{
    private ISushiListener _listener;
    private ISushiEngine _engine;
    
    public SushiHostedServer(ISushiListener listener, ISushiEngine engine)
    {
        _listener = listener;
        _engine = engine;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _engine.Start();
        _listener.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}