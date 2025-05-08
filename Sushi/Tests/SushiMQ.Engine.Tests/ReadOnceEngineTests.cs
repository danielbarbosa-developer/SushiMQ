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
using SushiMQ.Engine.Infrastructure;
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
        
        var expectedMessage = Encoding.UTF8.GetBytes(_json);

        // Act
        
        readOnceEngine.AddMessage(2750574782, expectedMessage);
        var sushiLineStatus = readOnceEngine.GetSushiLineStatus(2750574782);

        // Assert
        sushiLineStatus.Active.Should().BeTrue();
        sushiLineStatus.UnreadMessages.Should().Be(1);
    }
    
    [Fact(DisplayName = "AddMessage Method should be thread-safe when adding BSON Messages concurrently")]
    public void AddMessage_Method_Should_Handle_Concurrent_Additions_Successfully()
    {
        // Arrange
        var readOnceEngine = new ReadOnceEngine(_config);
        readOnceEngine.Start();
    
        var expectedMessage = Encoding.UTF8.GetBytes(_json);

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
    
    // Reading Messages operations
    [Fact(DisplayName = "ReadMessage Method should filter by sushi line queue hash adding BSON Message")]
    public void ReadMessage_Method_Should_Filter_By_SushiLineHash_Reading_and_Removing_Bson_Message_With_Success()
    {
        // Arrange
        var readOnceEngine = new ReadOnceEngine(_config);
        readOnceEngine.Start();
    
        var expectedMessage = Encoding.UTF8.GetBytes(_json);
        const uint sushiLineHash = 2750574782;
        
        readOnceEngine.AddMessage(sushiLineHash, expectedMessage);

        // Act
        
        var actualMessage = readOnceEngine.ReadMessage(sushiLineHash);
        var sushiLineStatus = readOnceEngine.GetSushiLineStatus(sushiLineHash);

        // Assert
        sushiLineStatus.Active.Should().BeTrue();
        sushiLineStatus.UnreadMessages.Should().Be(0);
        
        actualMessage.Should().BeEquivalentTo(expectedMessage);
    }
    
    [Fact(DisplayName = "ReadMessage Method should be thread-safe when adding BSON Messages concurrently")]
    public void ReadMessage_Method_Should_Handle_Concurrent_Reading_Successfully()
    {
        // Arrange
        var readOnceEngine = new ReadOnceEngine(_config);
        readOnceEngine.Start();
    
        var expectedMessage = Encoding.UTF8.GetBytes(_json);

        const int parallelTasks = 100000000;
        const uint sushiLineHash = 2750574782;
        
        Parallel.For((long)0, parallelTasks, _ =>
        {
            readOnceEngine.AddMessage(sushiLineHash, expectedMessage);
        });
        
        
        // Act
        var beforeSushiLineStatus = readOnceEngine.GetSushiLineStatus(sushiLineHash);
        
        Parallel.For((long)0, parallelTasks, _ =>
        {
            readOnceEngine.ReadMessage(sushiLineHash);
        });

        var actualSushiLineStatus = readOnceEngine.GetSushiLineStatus(sushiLineHash);

        // Assert
        beforeSushiLineStatus.Active.Should().BeTrue();
        beforeSushiLineStatus.UnreadMessages.Should().Be(parallelTasks);
        
        actualSushiLineStatus.Active.Should().BeTrue();
        actualSushiLineStatus.UnreadMessages.Should().Be(0);
    }

    
    
    // TODO: Error handling scenarios 
}