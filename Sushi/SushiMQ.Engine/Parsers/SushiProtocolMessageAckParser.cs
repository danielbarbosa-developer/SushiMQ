using System.Buffers.Binary;
using SushiMQ.Engine.Dtos;

namespace SushiMQ.Engine.Parsers;

public static class SushiProtocolMessageAckParser
{
    public static Memory<byte> ToBuffer(SushiProtocolMessageAck ack)
    {
        Span<byte> buffer = stackalloc byte[12];
        
        BinaryPrimitives.WriteInt64LittleEndian(buffer.Slice(0, 8), ack.Timestamp);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(8, 4), ack.SushiLineHash);
        
        return buffer.ToArray();
    }
}