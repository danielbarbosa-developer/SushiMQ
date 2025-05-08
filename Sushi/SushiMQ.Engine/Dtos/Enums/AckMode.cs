namespace SushiMQ.Engine.Dtos.Enums;

public enum AckMode : byte
{
    None = 0x00,
    Leader = 0x01,
    All = 0x02 
}
