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
