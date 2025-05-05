namespace SushiMQ.Engine.Dtos.Enums;

public enum AcknowledgeMode : byte
{
    None = 0x00,
    Leader = 0x01,
    All = 0x02 
}
