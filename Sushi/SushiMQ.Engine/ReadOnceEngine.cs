namespace SushiMQ.Engine;

public interface ISushiEngine
{
    void Start();
    void AddMessage(uint sushiLineHash, byte[] message);
    void ReadMessage(uint sushiLineHash);
    void Stop();
}
public class ReadOnceEngine : ISushiEngine
{
    public void Start()
    {
        throw new NotImplementedException();
    }

    public void AddMessage(uint sushiLineHash, byte[] message)
    {
        throw new NotImplementedException();
    }

    public void ReadMessage(uint sushiLineHash)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }
}