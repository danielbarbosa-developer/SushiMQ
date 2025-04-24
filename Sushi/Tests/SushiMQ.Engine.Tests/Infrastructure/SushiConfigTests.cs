using FluentAssertions;
using SushiMQ.Engine.Infrastructure;

namespace SushiMQ.Engine.Tests.Infrastructure;

public class SushiConfigTests
{
    private readonly SushiLineMetadataDto[] _expectedSushiLineMetadataArray;

    public SushiConfigTests()
    {
        _expectedSushiLineMetadataArray = ExpectedSushiLineMetadataArray();
    }
    
    [Fact(DisplayName = "GetSushiLineMetadata Should return Sushi line metadata")]
    public void GetSushiLineMetadata_Should_Return_SushiLineMetadataArray()
    {
        // Arrange
        var sushiConfig = new SushiConfig();
        
        // Act
        var sushiLineMetadataArray = sushiConfig.GetSushiLineMetadata();
        
        // Assert
        sushiLineMetadataArray.Should().NotBeNull();
        sushiLineMetadataArray.Length.Should().Be(2);
    }
    
    [Fact(DisplayName = "GetSushiLineMetadata Should return Sushi line metadata populated according to configuration file")]
    public void GetSushiLineMetadata_Should_Return_SushiLineMetadataArray_Populated_According_To_Configuration_File()
    {
        // Arrange
        var sushiConfig = new SushiConfig();
        
        // Act
        var sushiLineMetadataArray = sushiConfig.GetSushiLineMetadata();
        
        // Assert
        Assert.Collection(sushiLineMetadataArray, firstItem =>
        {
            firstItem.Should().NotBeNull();
            AssertSushiLine(firstItem, _expectedSushiLineMetadataArray[0]);
        }, secondItem =>
        {
            secondItem.Should().NotBeNull();
            AssertSushiLine(secondItem, _expectedSushiLineMetadataArray[1]);
        });
    }


    private void AssertSushiLine(SushiLineMetadataDto expectedSushiLineMetadataDto, SushiLineMetadataDto actualSushiLineMetadataDto)
    {
        actualSushiLineMetadataDto.SushiLineHash.Should().Be(expectedSushiLineMetadataDto.SushiLineHash);
        actualSushiLineMetadataDto.Consumption.Should().Be(expectedSushiLineMetadataDto.Consumption);
        actualSushiLineMetadataDto.Storage.Should().Be(expectedSushiLineMetadataDto.Storage);
    }
    
    private static SushiLineMetadataDto[] ExpectedSushiLineMetadataArray()
    {
        return
        [
            new SushiLineMetadataDto()
            {
                SushiLineHash = 2750574782,
                Consumption = 0,
                Storage = 0
            },
            new SushiLineMetadataDto()
            {
                SushiLineHash = 2230367815,
                Consumption = 2,
                Storage = 1
            }
        ];
    }
}