namespace SushiMQ.Engine.Infrastructure;

public class EngineStatusDto
{
    public int SushiLines { get; set; }
    public EngineStatus Status { get; set; }
}

public enum EngineStatus
{
    Running,
    Stopped,
    Failed,
}