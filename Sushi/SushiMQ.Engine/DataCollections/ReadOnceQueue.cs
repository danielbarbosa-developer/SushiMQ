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

namespace SushiMQ.Engine.DataCollections;

public class ReadOnceQueue : IDisposable
{
    public uint SushiLineHash { get; }
    public ConcurrentQueue<byte[]> Messages { get; }

    public ReadOnceQueue( uint sushiLineHash)
    {
        SushiLineHash = sushiLineHash;
        Messages = new ConcurrentQueue<byte[]>();
    }

    public void Enqueue(byte[] message)
    {
        if (message is null) throw new ArgumentNullException(nameof(message));
        Messages.Enqueue(message);
    }

    public bool TryDequeue(out byte[] message)
    {
        return Messages.TryDequeue(out message);
    }

    public void Dispose()
    {
        while (Messages.TryDequeue(out _)) { }
        Messages.Clear();
        GC.SuppressFinalize(this);
    }
}
