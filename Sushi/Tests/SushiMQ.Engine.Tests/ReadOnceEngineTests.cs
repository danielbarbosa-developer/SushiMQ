using FluentAssertions;
using SushiMQ.Engine.Infrastructure;
using TestUtils.BSON;
using TestUtils.Seeds;

namespace SushiMQ.Engine.Tests;

public class ReadOnceEngineTests
{
    private readonly ISushiConfig _config;
    private readonly string _json;

    public ReadOnceEngineTests()
    {
        _config = new SushiConfig();
        _json = JsonData.GenerateJson();
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
    
    // Adding Messages operations
    
    [Fact(DisplayName = "AddMessage Method should filter by sushi line queue hash adding BSON Message")]
    public void AddMessage_Method_Should_Filter_By_SushiLineHash_Adding_Bson_Message_With_Success()
    {
        // Arrange
        var readOnceEngine = new ReadOnceEngine(_config);
        readOnceEngine.Start();
        
        var expectedMessage = BsonConvertionHelper.ConvertJsonToBytes(_json);

        // Act
        
        readOnceEngine.AddMessage(2750574782, expectedMessage);
        var sushiLineStatus = readOnceEngine.GetSushiLineStatus(2750574782);

        // Assert
        sushiLineStatus.Active.Should().BeTrue();
        sushiLineStatus.UnreadMessages.Should().Be(1);
    }
    
    // Adding multi thread and concurrent tests
    
    [Fact(DisplayName = "AddMessage Method should be thread-safe when adding BSON Messages concurrently")]
    public void AddMessage_Method_Should_Handle_Concurrent_Additions_Successfully()
    {
        // Arrange
        var readOnceEngine = new ReadOnceEngine(_config);
        readOnceEngine.Start();
    
        var expectedMessage = BsonConvertionHelper.ConvertJsonToBytes(_json);

        const int parallelTasks = 100000000;
        const uint sushiLineHash = 2750574782;

        // Act
        Parallel.For((long)0, parallelTasks, _ =>
        {
            readOnceEngine.AddMessage(sushiLineHash, expectedMessage);
        });

        var sushiLineStatus = readOnceEngine.GetSushiLineStatus(sushiLineHash);

        // Assert
        sushiLineStatus.Active.Should().BeTrue();
        sushiLineStatus.UnreadMessages.Should().Be(parallelTasks);
    }

    
    
    // TODO: Error handling scenarios 
}