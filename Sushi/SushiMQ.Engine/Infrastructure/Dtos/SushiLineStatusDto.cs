namespace SushiMQ.Engine.Infrastructure;

public class SushiLineStatusDto
{
    public uint SushiLineHash { get; set; }
    public int UnreadMessages { get; set; }
    public bool Active { get; set; }
}