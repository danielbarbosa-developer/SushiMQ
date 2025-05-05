namespace SushiMQ.Engine.Dtos.Enums;

public enum SushiResourceType : byte
{
    Health = 0x00,
    Register = 0x01,
    Publish = 0x02,
    Consume = 0x03
}