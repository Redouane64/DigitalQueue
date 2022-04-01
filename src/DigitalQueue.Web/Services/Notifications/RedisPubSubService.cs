using StackExchange.Redis;

namespace DigitalQueue.Web.Services.Notifications;

public abstract class RedisPubSubService : IDisposable
{
    private readonly IConnectionMultiplexer _connection;

    protected RedisPubSubService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connection = connectionMultiplexer;
    }

    protected void Subscribe(string channel)
    {
        var subscriber = _connection.GetSubscriber();
        subscriber.Subscribe(new RedisChannel(channel, RedisChannel.PatternMode.Literal))
            .OnMessage(this.OnMessage);
    }

    protected virtual async Task Publish(string channel, string value)
    {
        var subscriber = _connection.GetSubscriber();
        await subscriber.PublishAsync(
            new RedisChannel(channel, RedisChannel.PatternMode.Literal), 
            new RedisValue(value)
        );
    }

    protected abstract Task OnMessage(ChannelMessage channelMessage);

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
