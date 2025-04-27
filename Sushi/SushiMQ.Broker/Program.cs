// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SushiMQ.Broker;
using SushiMQ.Broker.Interfaces;
using SushiMQ.Engine;
using SushiMQ.Engine.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

// Services Registration
builder.Services.AddSingleton<ISushiListener, SushiTcpListener>();
builder.Services.AddSingleton<ISushiEngine, ReadOnceEngine>();
builder.Services.AddSingleton<ISushiConfig, SushiConfig>();
builder.Services.AddHostedService<SushiHostedServer>();

// Logging
builder.Logging.AddConsole();

using var host = builder.Build();
await host.RunAsync();
