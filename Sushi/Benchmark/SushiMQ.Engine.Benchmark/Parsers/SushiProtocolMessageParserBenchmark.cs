using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using SushiMQ.Engine.Parsers;
using TestUtils.Seeds;

namespace SushiMQ.Engine.Benchmark.Parsers;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80, baseline: true)]
public class SushiProtocolMessageParserBenchmark
{
    private byte[] buffer;

    [GlobalSetup]
    public void Setup()
    {
        buffer = SushiProtocolMessageSeeder.GenerateSushiProtocolMessageBuffer(); 
    }

    [Benchmark]
    public void SushiProtocolMessageParser_FromBytes_With_Success()
    {
        _ = SushiProtocolMessageParser.FromBytes(buffer);
    }
    
    [Benchmark]
    public void SushiProtocolMessageParserWithSpan_FromBytes_With_Success()
    {
        _ = SushiProtocolMessageParser.FromBytesSpan(buffer);
    }

}