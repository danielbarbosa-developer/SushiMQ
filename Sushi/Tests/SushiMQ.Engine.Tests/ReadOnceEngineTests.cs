using FluentAssertions;
using SushiMQ.Engine.Infrastructure;

namespace SushiMQ.Engine.Tests;

public class ReadOnceEngineTests
{
    private readonly ISushiConfig _config;

    public ReadOnceEngineTests()
    {
        _config = new SushiConfig();
    }
    
    [Fact(DisplayName = "Start method should create read once sushi line queue based on configuration file")]
    public void Start_Method_Should_Initialize_ReadOnceQueue_SushiLine_With_Success()
    {
        // Arrange
        var readOnceEngine = new ReadOnceEngine(_config);

        // Act
        readOnceEngine.Start();
        var engineStatus = readOnceEngine.GetStatus();

        // Assert
        engineStatus.Status.Should().Be(EngineStatus.Running);
        engineStatus.SushiLines.Should().Be(1);
    }
    
    [Fact(DisplayName = "Get Method should create read once sushi line queue based on configuration file")]
    public void GetSushiLineStatus_Method_Should_Filter_By_SushiLineHash_Returning_SushiLine_Status_With_Success()
    {
        // Arrange
        var readOnceEngine = new ReadOnceEngine(_config);
        readOnceEngine.Start();

        // Act
        
        var sushiLineStatus = readOnceEngine.GetSushiLineStatus(2750574782);

        // Assert
        sushiLineStatus.Active.Should().BeTrue();
        sushiLineStatus.UnreadMessages.Should().Be(0);
    }
    
    // TODO: Error handling scenarios 
}