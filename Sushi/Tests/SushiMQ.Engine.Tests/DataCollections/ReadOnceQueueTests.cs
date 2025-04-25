using System.Text;
using FluentAssertions;
using Force.Crc32;
using SushiMQ.Engine.DataCollections;
using TestUtils.BSON;
using TestUtils.Seeds;

namespace SushiMQ.Engine.Tests.DataCollections;

public class ReadOnceQueueTests
{
    private readonly string _json;
    private readonly uint _sushiLineHash;
    
    public ReadOnceQueueTests()
    {
        _sushiLineHash = Crc32Algorithm.Compute(Encoding.UTF8.GetBytes("test.sushiLine.1"));
        _json = JsonData.GenerateJson();
    }
    
    [Fact]
    public void Should_Initialize_SingleConsumeQueue_With_Success()
    {
        // Arrange
        // Act
        var singleConsumeQueue = new ReadOnceQueue(_sushiLineHash);

        // Assert
        singleConsumeQueue.Messages.Count.Should().Be(0);
        singleConsumeQueue.SushiLineHash.Should().Be(_sushiLineHash);
    }
    
    [Fact]
    public void Should_Enqueue_BsonMessage_SingleConsumeQueue_With_Success()
    {
        // Arrange
        
        var singleConsumeQueue = new ReadOnceQueue(_sushiLineHash);

        var message = BsonConvertionHelper.ConvertJsonToBytes(_json);

        // Act
        singleConsumeQueue.Enqueue(message);

        // Assert
        singleConsumeQueue.Messages.Count.Should().Be(1);
    }
    
    [Fact]
    public void Should_Dequeue_BsonMessage_SingleConsumeQueue_With_Success()
    {
        // Arrange
        
        var singleConsumeQueue = new ReadOnceQueue(_sushiLineHash);

        var expectedMessage = BsonConvertionHelper.ConvertJsonToBytes(_json);
        var message = Array.Empty<byte>();

        // Act
        singleConsumeQueue.Enqueue(expectedMessage);
        var result = singleConsumeQueue.TryDequeue(out message);

        // Assert
        result.Should().BeTrue();
        message.Should().BeEquivalentTo(expectedMessage);
        singleConsumeQueue.Messages.Count.Should().Be(0);
    }
    
    // TODO: Error handling scenarios
    
    
}