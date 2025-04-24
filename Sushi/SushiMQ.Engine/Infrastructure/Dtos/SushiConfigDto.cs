namespace SushiMQ.Engine.Infrastructure;

public record SushiConfigDto : IDisposable
{
    public SushiServerConfig Server { get; set; }
    
    public SushiLineConfig[] SushiLines { get; set; }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

public record SushiServerConfig
{
    public string Host { get; set; }
    public int Port { get; set; }
}

public record SushiLineConfig
{
    public string SushiLineName { get; set; }
    public bool SushiLineStorage { get; set; }
    public SushiLineConsumption SushiLineConsumption { get; set; }
}


public enum SushiLineConsumption : byte
{
    ReadOnce,
    Fanout,
    ReadMany
}