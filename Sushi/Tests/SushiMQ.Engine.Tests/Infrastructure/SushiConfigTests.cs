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
    
    // TODO: Error handling scenarios
    
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