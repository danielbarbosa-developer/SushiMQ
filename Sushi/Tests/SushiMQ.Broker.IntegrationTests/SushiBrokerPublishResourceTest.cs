using System.Text;
using FluentAssertions;
using SushiMQ.Engine.Dtos;
using TestUtils.Seeds;

namespace SushiMQ.Broker.IntegrationTests;

public class SushiBrokerPublishResourceTest  : IClassFixture<SushiBrokerFixture>
{
    
    private readonly SushiBrokerFixture _fixture;
    
    public SushiBrokerPublishResourceTest(SushiBrokerFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task SushiBroker_Should_Return_Ack_On_Publish_Resource_With_Success()
    {
        // Arrange
        var expectedSushiLineHash = 3735928559U;
        using var client = await _fixture.CreateTcpClient();
        await using var stream = client.GetStream();
        
        var sushiProtocolMessage = SushiProtocolMessageSeeder.GenerateSushiProtocolMessageBuffer();
        var payloadLength = 1 + 1 + sushiProtocolMessage.Length;
        var payload = new byte[4 + payloadLength];
        BitConverter.GetBytes(payloadLength).CopyTo(payload, 0);
        payload[4] = 0x02; // Resource Type Publish
        payload[5] = 0x01; // Ack
        
        Array.Copy(sushiProtocolMessage, 0, payload, 6, sushiProtocolMessage.Length);
        
        // Act
        await stream.WriteAsync(payload);

        var responseBuffer = new byte[12];
        _ = await stream.ReadAsync(responseBuffer);
        
        // Assert
        var acknowledgeResponse = new SushiProtocolMessageAck
        {
            Timestamp = BitConverter.ToInt64(responseBuffer, 0),
            SushiLineHash = BitConverter.ToUInt32(responseBuffer, 8)
        };

        acknowledgeResponse.SushiLineHash.Should().Be(expectedSushiLineHash);

    }
}