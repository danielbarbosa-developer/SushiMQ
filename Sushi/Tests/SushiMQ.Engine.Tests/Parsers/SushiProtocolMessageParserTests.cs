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

using System.Text;
using SushiMQ.Engine.Parsers;
using TestUtils.Seeds;

namespace SushiMQ.Engine.Tests.Parsers;

public class SushiProtocolMessageParserTests
{
    [Fact(DisplayName = "Should convert bytes buffer to a full SushiProtocolMessage object with success")]
    public void Should_Convert_BytesBuffer_To_SushiProtocolMessage_Object_With_Success()
    {
        // Arrange
        var buffer = SushiProtocolMessageSeeder.GenerateSushiProtocolMessageBuffer();

        // Act
        var sushiProtocolMessage = SushiProtocolMessageParser.FromBytes(buffer);

        // Assert
        
        Assert.Equal(0xABCD, sushiProtocolMessage.Magic);
        Assert.Equal(1, sushiProtocolMessage.MessageType);

        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        Assert.InRange(sushiProtocolMessage.Timestamp, now - 5000, now + 5000);

        Assert.Equal(16u, sushiProtocolMessage.MessageCorrelationIdLength);
        Assert.NotNull(sushiProtocolMessage.MessageCorrelationId);
        Assert.Equal(16, sushiProtocolMessage.MessageCorrelationId!.Length);

        Assert.Equal(0xDEADBEEF, sushiProtocolMessage.SushiLineHash);

        var expectedLineName = "realtime-metrics";
        Assert.Equal((uint)expectedLineName.Length, sushiProtocolMessage.SushiLineNamePayloadLength);
        Assert.Equal(expectedLineName, Encoding.UTF8.GetString(sushiProtocolMessage.SushiLineName!));

        Assert.Equal(16u, sushiProtocolMessage.PayloadLength);
        Assert.NotNull(sushiProtocolMessage.Payload);
        for (int i = 0; i < 16; i++)
        {
            Assert.Equal((byte)(i + 1), sushiProtocolMessage.Payload![i]);
        }
    }
    
    [Fact(DisplayName = "Should convert bytes buffer to a full SushiProtocolMessage object using span with success")]
    public void Should_Convert_BytesBuffer_To_SushiProtocolMessage_Object_Using_Span_With_Success()
    {
        // Arrange
        var buffer = SushiProtocolMessageSeeder.GenerateSushiProtocolMessageBuffer();

        // Act
        var sushiProtocolMessage = SushiProtocolMessageParser.FromBytesSpan(buffer);

        // Assert
        
        Assert.Equal(0xABCD, sushiProtocolMessage.Magic);
        Assert.Equal(1, sushiProtocolMessage.MessageType);

        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        Assert.InRange(sushiProtocolMessage.Timestamp, now - 5000, now + 5000);

        Assert.Equal(16u, sushiProtocolMessage.MessageCorrelationIdLength);
        Assert.NotNull(sushiProtocolMessage.MessageCorrelationId);
        Assert.Equal(16, sushiProtocolMessage.MessageCorrelationId!.Length);

        Assert.Equal(0xDEADBEEF, sushiProtocolMessage.SushiLineHash);

        var expectedLineName = "realtime-metrics";
        Assert.Equal((uint)expectedLineName.Length, sushiProtocolMessage.SushiLineNamePayloadLength);
        Assert.Equal(expectedLineName, Encoding.UTF8.GetString(sushiProtocolMessage.SushiLineName!));

        Assert.Equal(16u, sushiProtocolMessage.PayloadLength);
        Assert.NotNull(sushiProtocolMessage.Payload);
        for (int i = 0; i < 16; i++)
        {
            Assert.Equal((byte)(i + 1), sushiProtocolMessage.Payload![i]);
        }
    }
}