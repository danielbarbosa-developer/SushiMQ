using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using SushiMQ.Engine.Infrastructure;
using TestUtils.BSON;
using TestUtils.Seeds;

namespace SushiMQ.Engine.Benchmark;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80, baseline: true)]
public class ReadOnceQueueBenchmark
{
    private byte[] bson;
    
    private uint sushiLineHash;
    
    private ISushiEngine sushiEngine;

    [GlobalSetup]
    public void Setup()
    {
        bson = BsonConvertionHelper.ConvertJsonToBytes(JsonData.GenerateJson());
        sushiLineHash = 2750574782;
        var config = new SushiConfig();
        sushiEngine = new ReadOnceEngine(config);
    }

    [Benchmark]
    public void ReadOnceQueue_SushiLine_AddMessage_With_Success()
    {
        sushiEngine.AddMessage(sushiLineHash, bson);
    }
    
    [Benchmark]
    public void ReadOnceQueue_SushiLine_AddMessage_Concurrency_With_Success()
    {
        const int parallelTasks = 100000000;
        Parallel.For(0, parallelTasks, _ =>
        {
            sushiEngine.AddMessage(sushiLineHash, bson);
        });
    }
    
    
}