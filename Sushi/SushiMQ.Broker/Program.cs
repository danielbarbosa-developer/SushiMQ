// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SushiMQ.Broker;
using SushiMQ.Broker.Interfaces;

var builder = Host.CreateApplicationBuilder(args);

// Services Registration
builder.Services.AddSingleton<ISushiListener, SushiTcpListener>();
builder.Services.AddHostedService<SushiHostedServer>();

// Logging
builder.Logging.AddConsole();

using var host = builder.Build();
await host.RunAsync();
