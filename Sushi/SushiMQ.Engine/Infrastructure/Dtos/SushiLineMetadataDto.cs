namespace SushiMQ.Engine.Infrastructure;

public record SushiLineMetadataDto
{
    public uint SushiLineHash { get; set; }
    public byte Storage { get; set; }
    public byte Consumption { get; set; } 
}