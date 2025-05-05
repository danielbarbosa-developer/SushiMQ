namespace SushiMQ.Broker.IntegrationTests;

public class SushiBrokerPublishResourceTest  : IClassFixture<SushiBrokerFixture>
{
    
    private readonly SushiBrokerFixture _fixture;
    
    public SushiBrokerPublishResourceTest(SushiBrokerFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void SushiBroker_Should_Return_Pong_On_Publish_Resource_With_Success()
    {
        
    }
}