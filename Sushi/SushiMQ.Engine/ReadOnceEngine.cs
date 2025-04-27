using System.Collections.Concurrent;
using SushiMQ.Engine.DataCollections;
using SushiMQ.Engine.Infrastructure;

namespace SushiMQ.Engine;

public interface ISushiEngine
{
    void Start();
    void AddMessage(uint sushiLineHash, byte[] message);
    void ReadMessage(uint sushiLineHash);
    void Stop();
    EngineStatusDto GetStatus();
    SushiLineStatusDto GetSushiLineStatus(uint sushiLineHash);
}
public class ReadOnceEngine : ISushiEngine
{
    private readonly ISushiConfig _config;
    
    private ConcurrentDictionary<uint, ReadOnceQueue> _readOnceQueues;
    
    private EngineStatus _engineStatus;
    public ReadOnceEngine(ISushiConfig config)
    {
        _engineStatus = EngineStatus.Stopped;
        _config = config;
        _readOnceQueues = new ConcurrentDictionary<uint, ReadOnceQueue>();
    }
    
    public void Start()
    {
        _engineStatus = EngineStatus.Running;

        var sushiLineMetadataArray = _config.GetSushiLineMetadata();

        for (int i = 0; i < sushiLineMetadataArray.Length; i++)
        {
            var sushiLineMetadata = sushiLineMetadataArray[i];

            if (sushiLineMetadata.Consumption == (byte)SushiLineConsumption.ReadOnce)
            {
                var readOnceQueue = new ReadOnceQueue(sushiLineMetadata.SushiLineHash);
                _readOnceQueues.TryAdd(sushiLineMetadata.SushiLineHash, readOnceQueue);
                
                // TODO: Define exception and unhappy path handlers (ongoing)
            }
        }
    }

    public void AddMessage(uint sushiLineHash, byte[] message)
    {
        if (_readOnceQueues.TryGetValue(sushiLineHash, out var queue))
        {
            queue.Enqueue(message);
        }
    }

    public void ReadMessage(uint sushiLineHash)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }

    public EngineStatusDto GetStatus()
    {
        var sushiLinesCount = _readOnceQueues.Count;
        var engineStatus = new EngineStatusDto()
        {
            Status = _engineStatus,
            SushiLines = sushiLinesCount
        };

        return engineStatus;
    }

    public SushiLineStatusDto GetSushiLineStatus(uint sushiLineHash)
    {
        if (_readOnceQueues.TryGetValue(sushiLineHash, out var queue))
        {
            return new SushiLineStatusDto()
            {
                SushiLineHash = queue.SushiLineHash,
                UnreadMessages = queue.Messages.Count,
                Active = true
            };
        }
        
        // TODO: Define exception and unhappy path handlers (ongoing)

        return new SushiLineStatusDto()
        {
            SushiLineHash = sushiLineHash,
            UnreadMessages = 0,
            Active = false

        };



    }
}