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
using FluentAssertions;
using Force.Crc32;
using SushiMQ.Engine.DataCollections;
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

        var message =  Encoding.UTF8.GetBytes(_json);


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

        var expectedMessage = Encoding.UTF8.GetBytes(_json);
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