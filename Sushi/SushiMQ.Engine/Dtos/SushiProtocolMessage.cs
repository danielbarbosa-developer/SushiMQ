namespace SushiMQ.Engine.Dtos;

public record SushiProtocolMessage
{
    public ushort Magic { get; set; }
    public byte MessageType { get; set; }
    public long Timestamp { get; set; }
    public uint MessageCorrelationIdLength { get; set; }
    public byte[]? MessageCorrelationId { get; set; }
    public uint SushiLineHash { get; set; }
    public uint SushiLineNamePayloadLength { get; set; }
    public byte[]? SushiLineName { get; set; }
    public uint PayloadLength { get; set; }
    public byte[]? Payload { get; set; }
}