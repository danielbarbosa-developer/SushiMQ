using System.Net.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SushiMQ.Broker.Interfaces;
using SushiMQ.Engine;
using SushiMQ.Engine.Infrastructure;

namespace SushiMQ.Broker.IntegrationTests;

public class SushiBrokerFixture : IAsyncLifetime
{
    public IHost Host { get; private set; }

    public async Task InitializeAsync()
    {
        Host = new HostBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<ISushiConfig, SushiConfig>();
                services.AddSingleton<ISushiEngine, ReadOnceEngine>();
                services.AddSingleton<ISushiListener, SushiTcpListener>();
                services.AddHostedService<SushiHostedServer>();
            })
            .Build();


        await Host.StartAsync();
        await Task.Delay(200); 
    }

    public async Task<TcpClient> CreateTcpClient()
    {
        var client = new TcpClient();
        await client.ConnectAsync("localhost", 5050);
        
        return client;
    }

    public async Task DisposeAsync()
    {
        await Host.StopAsync();
        Host.Dispose();
    }
}
