using System.Collections.Concurrent;

namespace SushiMQ.Engine.DataCollections;

public class SingleConsumeQueue : IDisposable
{
    public byte[] SushiLineName { get; }
    public uint SushiLineHash { get; }
    public ConcurrentQueue<byte[]> Messages { get; }

    public SingleConsumeQueue(byte[] sushiLineBytes, uint sushiLineHash)
    {
        SushiLineName = sushiLineBytes ?? throw new ArgumentNullException(nameof(sushiLineBytes));
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
