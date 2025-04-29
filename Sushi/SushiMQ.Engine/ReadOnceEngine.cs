// Sushi MQ
// Copyright (C) 2025 Danzopen and Daniel Barbosa
//
// This file is part of Sushi MQ.
//
// Sushi MQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, **version 3** of the License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see: <https://www.gnu.org/licenses/gpl-3.0.html>
//
// This license ensures that you can use, study, share, and improve this software
// freely, as long as you preserve this license and credit the original authors.

using System.Collections.Concurrent;
using SushiMQ.Engine.DataCollections;
using SushiMQ.Engine.Infrastructure;

namespace SushiMQ.Engine;

public interface ISushiEngine
{
    void Start();
    void AddMessage(uint sushiLineHash, byte[] message);
    byte[] ReadMessage(uint sushiLineHash);
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

    public byte[] ReadMessage(uint sushiLineHash)
    {
        if (!_readOnceQueues.TryGetValue(sushiLineHash, out var queue)) return Array.Empty<byte>();
        
        return queue.TryDequeue(out var message) ? message : [];
        
        // TODO: Define exception and unhappy path handlers (ongoing)
        // TODO; Define return type when queue is empty or whe has error
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