// Sushi MQ
// Copyright (C) 2025 Danzopen and Daniel Barbosa
//
// This file is part of Sushi MQ.
//
// Sushi MQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, **version 3** of the License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see: <https://www.gnu.org/licenses/gpl-3.0.html>
//
// This license ensures that you can use, study, share, and improve this software
// freely, as long as you preserve this license and credit the original authors.

using System.Buffers.Binary;
using SushiMQ.Engine.Dtos;

namespace SushiMQ.Engine.Parsers;

public static class SushiProtocolMessageParser
{
    /// <summary>
    /// Converts a byte array to a SushiProtocolMessage object.
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns>SushiProtocolMessage</returns>
    public static SushiProtocolMessage FromBytes(byte[] buffer)
    {
        using var ms = new MemoryStream(buffer);
        using var reader = new BinaryReader(ms);

        var message = new SushiProtocolMessage
        {
            Magic = reader.ReadUInt16(),
            MessageType = reader.ReadByte(),
            Timestamp = reader.ReadInt64(),
            MessageCorrelationIdLength = reader.ReadUInt32()
        };

        if (message.MessageCorrelationIdLength > 0)
        {
            message.MessageCorrelationId = reader.ReadBytes((int)message.MessageCorrelationIdLength);
        }

        message.SushiLineHash = reader.ReadUInt32();
        message.SushiLineNamePayloadLength = reader.ReadUInt32();

        if (message.SushiLineNamePayloadLength > 0)
        {
            message.SushiLineName = reader.ReadBytes((int)message.SushiLineNamePayloadLength);
        }

        message.PayloadLength = reader.ReadUInt32();

        if (message.PayloadLength > 0)
        {
            message.Payload = reader.ReadBytes((int)message.PayloadLength);
        }

        return message;
    }
    
    /// <summary>
    /// Converts a byte Span to a SushiProtocolMessage object.
    /// Using Span and BitConverter to gain higher performance, it could be 3x faster and use 2x less memory than FromBytes method in the same class 
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns>SushiProtocolMessage</returns>
    public static SushiProtocolMessage FromBytesSpan(ReadOnlySpan<byte> buffer)
    {
        var offset = 0;

        ushort magic = BitConverter.ToUInt16(buffer.Slice(offset, 2));
        offset += 2;

        byte messageType = buffer[offset];
        offset += 1;

        long timestamp = BitConverter.ToInt64(buffer.Slice(offset, 8));
        offset += 8;

        uint correlationIdLength = BitConverter.ToUInt32(buffer.Slice(offset, 4));
        offset += 4;

        byte[]? correlationId = null;
        if (correlationIdLength > 0)
        {
            correlationId = buffer.Slice(offset, (int)correlationIdLength).ToArray();
            offset += (int)correlationIdLength;
        }

        uint sushiLineHash = BitConverter.ToUInt32(buffer.Slice(offset, 4));
        offset += 4;

        uint sushiLineNameLength = BitConverter.ToUInt32(buffer.Slice(offset, 4));
        offset += 4;

        byte[]? sushiLineName = null;
        if (sushiLineNameLength > 0)
        {
            sushiLineName = buffer.Slice(offset, (int)sushiLineNameLength).ToArray();
            offset += (int)sushiLineNameLength;
        }

        uint payloadLength = BitConverter.ToUInt32(buffer.Slice(offset, 4));
        offset += 4;

        byte[]? payload = null;
        if (payloadLength > 0)
        {
            payload = buffer.Slice(offset, (int)payloadLength).ToArray();
            offset += (int)payloadLength;
        }

        return new SushiProtocolMessage
        {
            Magic = magic,
            MessageType = messageType,
            Timestamp = timestamp,
            MessageCorrelationIdLength = correlationIdLength,
            MessageCorrelationId = correlationId,
            SushiLineHash = sushiLineHash,
            SushiLineNamePayloadLength = sushiLineNameLength,
            SushiLineName = sushiLineName,
            PayloadLength = payloadLength,
            Payload = payload
        };
    }
    
    /// <summary>
    /// Converts a byte Span to a SushiProtocolMessage object.
    /// Using Span and BitConverter to gain higher performance, it could be 3x faster and use 2x less memory than FromBytes method in the same class 
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns>SushiProtocolMessage</returns>
    public static SushiProtocolMessage FromBytesSpanOptimized(ReadOnlySpan<byte> buffer)
    {
        var offset = 0;

        ushort magic = BinaryPrimitives.ReadUInt16LittleEndian(buffer.Slice(offset, 2));
        offset += 2;

        byte messageType = buffer[offset];
        offset += 1;

        long timestamp = BinaryPrimitives.ReadInt64LittleEndian(buffer.Slice(offset, 8));
        offset += 8;

        uint correlationIdLength = BinaryPrimitives.ReadUInt32LittleEndian(buffer.Slice(offset, 4));
        offset += 4;

        byte[]? correlationId = null;
        if (correlationIdLength > 0)
        {
            correlationId = buffer.Slice(offset, (int)correlationIdLength).ToArray();
            offset += (int)correlationIdLength;
        }

        uint sushiLineHash = BinaryPrimitives.ReadUInt32LittleEndian(buffer.Slice(offset, 4));
        offset += 4;

        uint sushiLineNameLength = BinaryPrimitives.ReadUInt32LittleEndian(buffer.Slice(offset, 4));
        offset += 4;

        byte[]? sushiLineName = null;
        if (sushiLineNameLength > 0)
        {
            sushiLineName = buffer.Slice(offset, (int)sushiLineNameLength).ToArray();
            offset += (int)sushiLineNameLength;
        }

        uint payloadLength = BinaryPrimitives.ReadUInt32LittleEndian(buffer.Slice(offset, 4));
        offset += 4;

        byte[]? payload = null;
        if (payloadLength > 0)
        {
            payload = buffer.Slice(offset, (int)payloadLength).ToArray();
            offset += (int)payloadLength;
        }

        return new SushiProtocolMessage
        {
            Magic = magic,
            MessageType = messageType,
            Timestamp = timestamp,
            MessageCorrelationIdLength = correlationIdLength,
            MessageCorrelationId = correlationId,
            SushiLineHash = sushiLineHash,
            SushiLineNamePayloadLength = sushiLineNameLength,
            SushiLineName = sushiLineName,
            PayloadLength = payloadLength,
            Payload = payload
        };
    }

}
