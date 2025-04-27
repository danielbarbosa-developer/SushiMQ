// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using SushiMQ.Engine.Benchmark;

Console.WriteLine("Hello, World!");

var summary = BenchmarkRunner.Run<ReadOnceQueueBenchmark>();