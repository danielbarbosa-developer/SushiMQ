namespace SushiMQ.Engine.Dtos;

public class SushiProtocolMessageAck
{
    public long Timestamp { get; set; }
    public uint SushiLineHash { get; set; }
}