using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using SushiMQ.Engine.Dtos;
using SushiMQ.Engine.Parsers;
using TestUtils.Seeds;

namespace SushiMQ.Engine.Benchmark.Parsers;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80, baseline: true)]
[SimpleJob(RuntimeMoniker.NativeAot80)]
public class SushiProtocolMessageAckParserBenchmark
{
    private SushiProtocolMessageAck sushiProtocolAck;

    [GlobalSetup]
    public void Setup()
    {
        sushiProtocolAck = new SushiProtocolMessageAck()
        {
            Timestamp = 1234567890,
            SushiLineHash = 1234567890
        };
    }

    [Benchmark]
    public void SushiProtocolMessageAckParser_ToBuffer_With_Success()
    {
        _ = SushiProtocolMessageAckParser.ToBuffer(sushiProtocolAck);
    }
}