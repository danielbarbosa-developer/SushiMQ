using System.Net.Sockets;
using System.Text;
using FluentAssertions;

namespace SushiMQ.Broker.IntegrationTests;

public class SushiBrokerHealthCheckResourceTests : IClassFixture<SushiBrokerFixture>
{
    private readonly SushiBrokerFixture _fixture;
    
    public SushiBrokerHealthCheckResourceTests(SushiBrokerFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = "SushiBroker Should Return Pong On HealthCheck Resource With Success")]
    public async Task SushiBroker_Should_Return_Pong_On_HealthCheck_Resource_With_Success()
    {
        // Arrange
        using var client = await _fixture.CreateTcpClient();
        await using var stream = client.GetStream();

        var payload = new byte[5];
        BitConverter.GetBytes(1).CopyTo(payload, 0);
        payload[4] = 0x00; // Health
        
        // Act
        await stream.WriteAsync(payload);
        
        var buffer = new byte[32];
        int read = await stream.ReadAsync(buffer);
        var response = Encoding.UTF8.GetString(buffer, 0, read);
        
        // Assert
        response.Should().Be("pong\n");
    }
    
    [Fact]
    public async Task SushiBroker_Should_Return_Pong_On_HealthCheck_Resource_In_Concurrency_Scenario_With_Success()
    {
        // Arrange
        const int parallelTasks = 10000;
        var tasks = Enumerable.Range(0, parallelTasks).Select(async _ =>
        {
            using var client = await _fixture.CreateTcpClient();
            await using var stream = client.GetStream();

            var payload = new byte[5];
            BitConverter.GetBytes(1).CopyTo(payload, 0);
            payload[4] = 0x00; // Health
            
            // Act
            await stream.WriteAsync(payload);

            var buffer = new byte[32];
            int read = await stream.ReadAsync(buffer);
            var response = Encoding.UTF8.GetString(buffer, 0, read);
            
            // Assert
            response.Should().Be("pong\n");
        });

        await Task.WhenAll(tasks);
    }

}