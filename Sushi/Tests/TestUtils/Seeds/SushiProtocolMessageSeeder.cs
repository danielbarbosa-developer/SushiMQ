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

using System;
using System.Text;

namespace TestUtils.Seeds
{
    public static class SushiProtocolMessageSeeder
    {
        public static byte[] GenerateSushiProtocolMessageBuffer(
            ushort magic = 0xABCD,
            byte messageType = 1,
            long? timestamp = null,
            Guid? correlationGuid = null,
            uint sushiLineHash = 0xDEADBEEF,
            string sushiLineName = "realtime-metrics",
            byte[]? payloadData = null)
        {
            timestamp ??= DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            correlationGuid ??= Guid.NewGuid();
            var correlationId = correlationGuid.Value.ToByteArray(); // 16 bytes

            payloadData ??= new byte[16];
            for (int i = 0; i < payloadData.Length; i++)
                payloadData[i] = (byte)(i + 1);

            var sushiLineNameBytes = Encoding.UTF8.GetBytes(sushiLineName);

            int totalLength =
                2 + // magic
                1 + // messageType
                8 + // timestamp
                4 + correlationId.Length + // correlationIdLength + correlationId
                4 + // sushiLineHash
                4 + sushiLineNameBytes.Length + // sushiLineNameLength + sushiLineName
                4 + payloadData.Length; // payloadLength + payload

            var buffer = new byte[totalLength];
            var offset = 0;

            BitConverter.GetBytes(magic).CopyTo(buffer, offset);
            offset += 2;

            buffer[offset++] = messageType;

            BitConverter.GetBytes(timestamp.Value).CopyTo(buffer, offset);
            offset += 8;

            BitConverter.GetBytes((uint)correlationId.Length).CopyTo(buffer, offset);
            offset += 4;

            correlationId.CopyTo(buffer.AsSpan(offset));
            offset += correlationId.Length;

            BitConverter.GetBytes(sushiLineHash).CopyTo(buffer, offset);
            offset += 4;

            BitConverter.GetBytes((uint)sushiLineNameBytes.Length).CopyTo(buffer, offset);
            offset += 4;

            sushiLineNameBytes.CopyTo(buffer.AsSpan(offset));
            offset += sushiLineNameBytes.Length;

            BitConverter.GetBytes((uint)payloadData.Length).CopyTo(buffer, offset);
            offset += 4;

            payloadData.CopyTo(buffer.AsSpan(offset));

            return buffer;
        }
    }
}
