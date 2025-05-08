using FluentAssertions;
using SushiMQ.Engine.Dtos;
using SushiMQ.Engine.Parsers;

namespace SushiMQ.Engine.Tests.Parsers;

public class SushiProtocolMessageAckParserTests
{
    [Fact]
    public void SushiProtocolMessageAckParser_ToBuffer_With_Success()
    {
        // Arrange
        const int expectedArrayCount = 12;

        var sushiProtocolAck = new SushiProtocolMessageAck()
        {
            Timestamp = 1234567890,
            SushiLineHash = 1234567890
        };

        // Act
        var ackBuffer = SushiProtocolMessageAckParser.ToBuffer(sushiProtocolAck);

        // Assert
        ackBuffer.Length.Should().Be(expectedArrayCount);
    }
}